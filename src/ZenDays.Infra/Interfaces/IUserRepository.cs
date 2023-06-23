using ZenDays.Domain.Entities;

namespace ZenDays.Infra.Interfaces
{
    public interface IUserRepository : IBaseRepository<Usuario>
    {
        Task<Usuario?> GetByName(string name, bool isEnabled = true);

        Task<Usuario?> GetByEmail(string email, bool isEnabled = true);
    }
}
