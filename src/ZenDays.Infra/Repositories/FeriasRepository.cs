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
		public async Task<List<Ferias>> GetAllFerias(string? userId, string? tipoUsuario, string? idDepartamento, string? idUsuarioExcluir, string? status, string? tipoUsuarioExcluir)
		{
			var usuariosQuery = _fireStoreDb.Collection(typeof(Usuario).Name).WhereNotEqualTo("Nome", "");

			if (!string.IsNullOrEmpty(idDepartamento)) usuariosQuery = usuariosQuery.WhereEqualTo("IdDepartamento", idDepartamento);
			if (!string.IsNullOrEmpty(tipoUsuario)) usuariosQuery = usuariosQuery.WhereEqualTo("TipoUsuario", int.Parse(tipoUsuario));
			if (!string.IsNullOrEmpty(tipoUsuarioExcluir)) usuariosQuery = usuariosQuery.WhereNotEqualTo("TipoUsuario", int.Parse(tipoUsuarioExcluir));

			var usuariosSnapshot = await usuariosQuery.GetSnapshotAsync();
			var idsUsuarios = usuariosSnapshot.Documents.Select(doc => doc.Id).ToList();

			if (idsUsuarios.Count() == 0) return new List<Ferias>();
			//ferias dos usuarios filtrados
			var feriasQuery = _fireStoreDb.Collection(typeof(Ferias).Name)
				.WhereIn("IdUsuario", idsUsuarios);

			if (!string.IsNullOrEmpty(status)) feriasQuery = feriasQuery.WhereEqualTo("Status", int.Parse(status));
			if (!string.IsNullOrEmpty(idUsuarioExcluir)) feriasQuery = feriasQuery.WhereNotEqualTo("IdUsuario", idUsuarioExcluir);
			if (!string.IsNullOrEmpty(userId)) feriasQuery = feriasQuery.WhereEqualTo("IdUsuario", userId);

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
