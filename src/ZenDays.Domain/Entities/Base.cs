using Google.Cloud.Firestore;

namespace ZenDays.Domain.Entities
{
    public class Base
    {
        public string Id { get; set; } = null!;
        [FirestoreProperty("Ativo")]
        public bool Ativo { get; set; } = true;
    }
}
