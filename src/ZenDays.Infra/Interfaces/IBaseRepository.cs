using ZenDays.Domain.Entities;

namespace ZenDays.Infra.Interfaces
{
	public interface IBaseRepository<T> where T : Base
	{
		Task<T?> Create(Dictionary<string, object> obj);
		Task<T?> Update(Dictionary<string, object> obj, string id);
		Task Disable(Dictionary<string, object> obj, string id);
        Task Delete(Dictionary<string, object> obj, string id);
        Task DeleteFromFirebaseAuth(string uid);
        Task<List<T>> GetAll();
		Task<List<T>> GetAll(List<(string field, string value)> filtros);

		Task<T?> Get(string id, bool isEnabled = true);
	}
}
