namespace ZenDays.Service.DTO
{
    public class FeriasDTO : BaseDTO
    {

        public string DataInicio { get; set; } = null!;

        public string DataFim { get; set; } = null!;

        public string DataPedido { get; set; } = "";

        public string DataValidacao { get; set; } = "";

        public string IdUsuario { get; set; } = "";
        public string NomeUsuario { get; set; } = "";
        public string NomeDepartamento { get; set; } = "";
        public int DiasVendidos { get; set; } 

        public int Status { get; set; } 

        public string? Mensagem { get; set; }
    }
}
