using Google.Cloud.Firestore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using ZenDays.Domain.Entities;
using ZenDays.Infra.Interfaces;

namespace ZenDays.Infra.Repositories
{
    public class UserRepository : BaseRepository<Usuario>, IUserRepository
    {
        public UserRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public async Task<Usuario?> GetByEmail(string email, bool isEnabled = true)
        {
            DocumentSnapshot? documentSnapshot = null!;
            Query query = _fireStoreDb.Collection(typeof(Usuario).Name)
                .WhereEqualTo("Email", email)
                .WhereEqualTo("Ativo", isEnabled);
            QuerySnapshot? snapshot = await query.GetSnapshotAsync();
            documentSnapshot = snapshot.Documents.FirstOrDefault();

            if (documentSnapshot != null)
            {
                Dictionary<string, object> entity = documentSnapshot.ToDictionary();
                string json = JsonConvert.SerializeObject(entity);
                var newEntity = JsonConvert.DeserializeObject<Usuario>(json);
                if (newEntity == null) return default;
                newEntity.Id = documentSnapshot.Id;
                return newEntity;
            }
            return default;
        }

        public async Task<Usuario?> GetByName(string name, bool isEnabled = true)
        {
            DocumentSnapshot? documentSnapshot = null!;
            Query query = _fireStoreDb.Collection(typeof(Usuario).Name)
                .WhereEqualTo("Nome", name)
                .WhereEqualTo("Ativo", isEnabled);
            QuerySnapshot? snapshot = await query.GetSnapshotAsync();
            documentSnapshot = snapshot.Documents.FirstOrDefault();

            if (documentSnapshot != null)
            {
                Dictionary<string, object> entity = documentSnapshot.ToDictionary();
                string json = JsonConvert.SerializeObject(entity);
                var newEntity = JsonConvert.DeserializeObject<Usuario>(json);
                if (newEntity == null) return default;
                newEntity.Id = documentSnapshot.Id;
                return newEntity;
            }
            return default;
        }
    }
}
