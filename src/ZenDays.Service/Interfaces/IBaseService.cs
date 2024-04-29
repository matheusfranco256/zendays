using ZenDays.Core.Models;
using ZenDays.Domain.Entities;
using ZenDays.Service.DTO;

namespace ZenDays.Service.Interfaces
{
	public interface IBaseService<T, J>

		 where T : Base
		 where J : BaseDTO
	{
		Task<ResultViewModel> Disable(Dictionary<string, object> obj, string id);
        Task<ResultViewModel> Delete(Dictionary<string, object> obj, string id);
        Task<ResultViewModel> DeleteFromFirebaseAuth(string uid);
        Task<ResultViewModel> Enable(Dictionary<string, object> obj, string id);
		Task<ResultViewModel> Create(Dictionary<string, object> obj);
		Task<ResultViewModel> Update(Dictionary<string, object> obj, string id);
		Task<ResultViewModel> GetAll();
		Task<ResultViewModel> GetAll(List<(string field, string value)> filtro);

		Task<ResultViewModel> Get(string id);
	}
}
