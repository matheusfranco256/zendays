using System.ComponentModel.DataAnnotations;

namespace ZenDays.Core.Models
{
    public class ResetPasswordViewModel
    {
        public string Email { get; set; } = null!;
        [Required]
        public string NovaSenha { get; set; } = null!;
        public string SenhaAntiga { get; set; } = "";
    }
}
