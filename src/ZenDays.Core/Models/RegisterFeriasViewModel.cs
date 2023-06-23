namespace ZenDays.Core.Models
{
    public class RegisterFeriasViewModel
    {
        public string DataInicio { get; set; } = null!;

        public string DataFim { get; set; } = null!;

        public string DataPedido { get; set; } = null!;

        public string DataValidacao { get; set; } = null!;

        public string IdUsuario { get; set; } = null!;

        public int DiasVendidos { get; set; }

        public int Status { get; set; }

        public string? Mensagem { get; set; }
    }
}
