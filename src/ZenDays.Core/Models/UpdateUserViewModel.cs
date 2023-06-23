namespace ZenDays.Core.Models
{
    public class UpdateUserViewModel
    {
        public string Id { get; set; } = null!;
        public string Nome { get; set; } = null!;

        public string CPF { get; set; } = null!;

        public string? Endereco { get; set; }

        public double Salario { get; set; }

        public string? Telefone { get; set; }

        public string DataNascimento { get; set; } = null!;

        public string UltimasFerias { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string IdDepartamento { get; set; } = null!;

        public string Cargo { get; set; } = null!;
    }
}
