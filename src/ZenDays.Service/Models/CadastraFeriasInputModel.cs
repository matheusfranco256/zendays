namespace ZenDays.Service.Models
{
	public class CadastraFeriasInputModel
	{
		public string DataInicio { get; set; } = null!;

		public string DataFim { get; set; } = null!;

		public string IdUsuario { get; set; } = null!;

		public int DiasVendidos { get; set; }

		public string? Mensagem { get; set; }
	}
}
