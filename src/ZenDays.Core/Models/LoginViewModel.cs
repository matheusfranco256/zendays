using System.ComponentModel.DataAnnotations;

namespace ZenDays.Core.Models
{
    public class LoginViewModel
    {
        [Required]
        public string Email { get; set; } = null!;
        [Required]
        public string Senha { get; set; } = null!;

        public LoginViewModel(string email, string senha)
        {
            Email = email;
            Senha = senha;
        }
    }
}
