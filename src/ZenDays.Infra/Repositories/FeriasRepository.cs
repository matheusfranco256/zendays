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
			if (!string.IsNullOrEmpty(status)) query = query.WhereEqualTo("Status", status);



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

		public async Task<List<Ferias>> GetAllFeriasByDepartamento(string idDepartamento, string? status)
		{
			//.WhereEqualTo(FieldPath.DocumentId, id)
			//usuarios com o Id do departamento
			var usuariosQuery = _fireStoreDb.Collection("Usuario")
				.WhereEqualTo("IdDepartamento", idDepartamento);
			var usuariosSnapshot = await usuariosQuery.GetSnapshotAsync();
			var idsUsuarios = usuariosSnapshot.Documents.Select(doc => doc.Id).ToList();

			//ferias dos usuarios filtrados
			var feriasQuery = _fireStoreDb.Collection("ferias")
				.WhereIn("IdUsuario", idsUsuarios);

			if (!string.IsNullOrEmpty(status)) feriasQuery = feriasQuery.WhereEqualTo("Status", status);

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

		public async Task<List<Ferias>> GetAllFeriasByTipoUsuario(string tipoUsuario, string? status)
		{
			//usuarios com o Id do departamento
			var usuariosQuery = _fireStoreDb.Collection("Usuario")
				.WhereEqualTo("TipoUsuario", tipoUsuario);
			var usuariosSnapshot = await usuariosQuery.GetSnapshotAsync();
			var idsUsuarios = usuariosSnapshot.Documents.Select(doc => doc.Id).ToList();

			//ferias dos usuarios filtrados
			var feriasQuery = _fireStoreDb.Collection("Ferias")
				.WhereIn("IdUsuario", idsUsuarios);

			if (!string.IsNullOrEmpty(status)) feriasQuery = feriasQuery.WhereEqualTo("Status", status);

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
