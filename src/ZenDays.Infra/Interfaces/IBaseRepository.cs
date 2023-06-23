using ZenDays.Domain.Entities;

namespace ZenDays.Infra.Interfaces
{
    public interface IBaseRepository<T> where T : Base
    {
        Task<T?> Create(Dictionary<string, object> obj);
        Task<T?> Update(Dictionary<string, object> obj, string id);
        Task Disable(Dictionary<string, object> obj, string id);
        Task<List<T>> GetAll();
        Task<T?> Get(string id, bool isEnabled = true);
    }
}
