using ZenDays.Core.Models;

namespace ZenDays.Service.Interfaces
{
    public interface IAuthService
    {

        Task<ResultViewModel> AuthenticateUser(LoginViewModel model);
        Task<ResultViewModel> ResetPassword(string email, string senhaAntiga, string novaSenha);
    }
}
