using Google.Cloud.Firestore;

namespace ZenDays.Domain.Entities
{
    public class Ferias : Base
    {
        [FirestoreProperty("DataInicio")]
        public string DataInicio { get; private set; } = null!;
        [FirestoreProperty("DataFim")]
        public string DataFim { get; private set; } = null!;
        [FirestoreProperty("DataPedido")]
        public string DataPedido { get; private set; } = null!;
        [FirestoreProperty("DataValidacao")]
        public string DataValidacao { get; private set; } = null!;
        [FirestoreProperty("IdUsuario")]
        public string IdUsuario { get; private set; } = null!;
        [FirestoreProperty("NomeUsuario")]
        public string NomeUsuario { get; private set; } = null!;
        [FirestoreProperty("NomeDepartamento")]
        public string NomeDepartamento { get; private set; } = null!;
        [FirestoreProperty("DiasVendidos")]
        public int DiasVendidos { get; private set; }
        [FirestoreProperty("Status")]
        public int Status { get; set; }
        [FirestoreProperty("Mensagem")]
        public string? Mensagem { get; private set; }

      

        public Ferias(string dataInicio, string dataFim, string dataPedido, string dataValidacao, string idUsuario,string nomeUsuario,string nomeDepartamento, int diasVendidos, int status, string? mensagem)
        {
            DataInicio = dataInicio;
            DataFim = dataFim;
            DataPedido = dataPedido;
            DataValidacao = dataValidacao;
            IdUsuario = idUsuario;
            NomeUsuario = nomeUsuario;
            NomeDepartamento = nomeDepartamento;
            DiasVendidos = diasVendidos;
            Status = status;
            Mensagem = mensagem;
        }
    }
}
