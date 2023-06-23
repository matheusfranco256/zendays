using Google.Cloud.Firestore;

namespace ZenDays.Domain.Entities
{
    public class Usuario : Base
    {
        [FirestoreProperty("Nome")]
        public string Nome { get; private set; } = null!;
        [FirestoreProperty("CPF")]
        public string CPF { get; private set; } = null!;
        [FirestoreProperty("Endereco")]
        public string? Endereco { get; private set; }
        [FirestoreProperty("Salario")]
        public double Salario { get; private set; }
        [FirestoreProperty("Telefone")]
        public string? Telefone { get; private set; }
        [FirestoreProperty("DataNascimento")]
        public string DataNascimento { get; private set; }
        [FirestoreProperty("UltimasFerias")]
        public string UltimasFerias { get; private set; }
        [FirestoreProperty("Email")]
        public string Email { get; private set; } = null!;
        [FirestoreProperty("Senha")]
        public string Senha { get; set; } = null!;
        [FirestoreProperty("IdDepartamento")]
        public string IdDepartamento { get; private set; } = null!;
        [FirestoreProperty("Cargo")]
        public string Cargo { get; private set; } = null!;
        [FirestoreProperty("TipoUsuario")]
        public int TipoUsuario { get; private set; }

        public Usuario(string nome, string cPF, string? endereco, double salario, string? telefone, string dataNascimento, string ultimasFerias, string email, string senha, string idDepartamento, string cargo, int tipoUsuario)
        {
            Nome = nome;
            CPF = cPF;
            Endereco = endereco;
            Salario = salario;
            Telefone = telefone;
            DataNascimento = dataNascimento;
            UltimasFerias = ultimasFerias;
            Email = email;
            Senha = senha;
            IdDepartamento = idDepartamento;
            Cargo = cargo;
            TipoUsuario = tipoUsuario;
        }
    }
}
