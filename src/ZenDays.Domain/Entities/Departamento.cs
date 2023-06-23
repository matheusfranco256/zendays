using Google.Cloud.Firestore;

namespace ZenDays.Domain.Entities
{
    public class Departamento : Base
    {
        [FirestoreProperty("Nome")]
        public string Nome { get; set; } = null!;
        public Departamento(string nome)
        {
            Nome = nome;
        }
    }
}