using ZenDays.Core.Models;
using ZenDays.Domain.Entities;
using ZenDays.Service.DTO;

namespace ZenDays.Service.Interfaces
{
    public interface IUserService : IBaseService<Usuario, BaseDTO>
    {
        Task<ResultViewModel> GetByName(string name);
        Task<ResultViewModel> GetByEmail(string email);

        Task<ResultViewModel> CreateUser(UsuarioDTO obj);
        Task<ResultViewModel> UpdateUser(UsuarioDTO obj);
        Task<ResultViewModel> DisableUser(string id);
        //Task<ResultViewModel> GetPaginated(FilterUserModel model);
    }
}
