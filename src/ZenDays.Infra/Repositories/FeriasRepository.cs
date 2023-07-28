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

        public async Task<List<Ferias>> GetAllFerias(string? userId)
        {
            Query query = _fireStoreDb.Collection(typeof(Ferias).Name);

            if (!string.IsNullOrEmpty(userId)) query = query.WhereEqualTo("IdUsuario", userId);



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

    }
}
