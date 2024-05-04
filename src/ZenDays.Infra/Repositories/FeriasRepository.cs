using Google.Cloud.Firestore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using ZenDays.Domain.Entities;
using ZenDays.Infra.Interfaces;

namespace ZenDays.Infra.Repositories
{
	public class FeriasRepository : BaseRepository<Ferias>, IFeriasRepository
	{
		public FeriasRepository(IConfiguration configuration, IHostingEnvironment environment) : base(configuration, environment)
		{
		}

		public async Task<List<Ferias>> GetAllFerias(string? userId, string? status)
		{
			Query query = _fireStoreDb.Collection(typeof(Ferias).Name);

			if (!string.IsNullOrEmpty(userId)) query = query.WhereEqualTo("IdUsuario", userId);
			if (!string.IsNullOrEmpty(status)) query = query.WhereEqualTo("Status", int.Parse(status));



			QuerySnapshot QuerySnapshot = await query.GetSnapshotAsync();
			List<Ferias> entitys = new();

			foreach (DocumentSnapshot documentSnapshot in QuerySnapshot.Documents)
			{
				if (documentSnapshot.Exists)
				{
					Dictionary<string, object> entity = documentSnapshot.ToDictionary();
					string json = JsonConvert.SerializeObject(entity);
					var newEntity = JsonConvert.DeserializeObject<Ferias>(json);
					if (newEntity != null)
					{
						newEntity.Id = documentSnapshot.Id;
						entitys.Add(newEntity);
					}
				}
			}
			return entitys;
		}

		public async Task<List<Ferias>> GetAllFeriasByDepartamento(string? idDepartamento, string? status)
		{
			Query feriasQuery = _fireStoreDb.Collection(typeof(Ferias).Name);
			if (!string.IsNullOrEmpty(idDepartamento))
			{

				//usuarios com o Id do departamento
				var usuariosQuery = _fireStoreDb.Collection(typeof(Usuario).Name)
					.WhereEqualTo("IdDepartamento", idDepartamento);
				var usuariosSnapshot = await usuariosQuery.GetSnapshotAsync();
				var idsUsuarios = usuariosSnapshot.Documents.Select(doc => doc.Id).ToList();

				if (idsUsuarios.Count() == 0) return new List<Ferias>();
				//ferias dos usuarios filtrados
				feriasQuery = feriasQuery.WhereIn("IdUsuario", idsUsuarios);
			}
			if (!string.IsNullOrEmpty(status)) feriasQuery = feriasQuery.WhereEqualTo("Status", int.Parse(status));

			var feriasSnapshot = await feriasQuery.GetSnapshotAsync();
			List<Ferias> entitys = new();

			foreach (DocumentSnapshot documentSnapshot in feriasSnapshot.Documents)
			{
				if (documentSnapshot.Exists)
				{
					Dictionary<string, object> entity = documentSnapshot.ToDictionary();
					string json = JsonConvert.SerializeObject(entity);
					var newEntity = JsonConvert.DeserializeObject<Ferias>(json);
					if (newEntity != null)
					{
						newEntity.Id = documentSnapshot.Id;
						entitys.Add(newEntity);
					}
				}
			}
			return entitys;
		}

		
        public async Task<List<Ferias>> GetAllFeriasByTipoUsuario(string tipoUsuario, string? idDepartamento, string? idUsuario, string? dataInicio, string? dataFim, string? status)
        {
            //usuarios por tipo
            var usuariosQuery = _fireStoreDb.Collection(typeof(Usuario).Name)
                .WhereEqualTo("TipoUsuario", int.Parse(tipoUsuario));
            if (!string.IsNullOrEmpty(idDepartamento)) usuariosQuery = usuariosQuery.WhereEqualTo("IdDepartamento", idDepartamento);

            var usuariosSnapshot = await usuariosQuery.GetSnapshotAsync();
            var idsUsuarios = usuariosSnapshot.Documents.Select(doc => doc.Id).ToList();

            if (idsUsuarios.Count() == 0) return new List<Ferias>();
            //ferias dos usuarios filtrados
            var feriasQuery = _fireStoreDb.Collection(typeof(Ferias).Name)
                .WhereIn("IdUsuario", idsUsuarios);

            if (!string.IsNullOrEmpty(status)) feriasQuery = feriasQuery.WhereEqualTo("Status", int.Parse(status));
            if (!string.IsNullOrEmpty(idUsuario)) feriasQuery = feriasQuery.WhereNotEqualTo("IdUsuario", idUsuario);
            if (!string.IsNullOrEmpty(dataInicio)) feriasQuery = feriasQuery.WhereGreaterThanOrEqualTo("DataInicio", dataInicio);
            if (!string.IsNullOrEmpty(dataFim)) feriasQuery = feriasQuery.WhereGreaterThanOrEqualTo("DataFim", dataFim);
            var feriasSnapshot = await feriasQuery.GetSnapshotAsync();
            List<Ferias> entitys = new();

            foreach (DocumentSnapshot documentSnapshot in feriasSnapshot.Documents)
            {
                if (documentSnapshot.Exists)
                {
                    Dictionary<string, object> entity = documentSnapshot.ToDictionary();
                    string json = JsonConvert.SerializeObject(entity);
                    var newEntity = JsonConvert.DeserializeObject<Ferias>(json);
                    if (newEntity != null)
                    {
                        newEntity.Id = documentSnapshot.Id;
                        entitys.Add(newEntity);
                    }
                }
            }
            return entitys;
        }
    }
}
