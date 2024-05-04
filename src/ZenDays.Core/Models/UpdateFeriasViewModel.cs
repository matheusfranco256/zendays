namespace ZenDays.Core.Models
{
    public class UpdateFeriasViewModel
    {
        public string Id { get; set; } = null!;
        public string DataInicio { get; set; } = null!;

        public string DataFim { get; set; } = null!;

        public int DiasVendidos { get; set; }

        public string? Mensagem { get; set; }
    }
}
