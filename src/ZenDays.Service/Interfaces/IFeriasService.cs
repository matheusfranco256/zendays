using ZenDays.Core.Models;
using ZenDays.Core.Utilities;
using ZenDays.Domain.Entities;
using ZenDays.Service.DTO;

namespace ZenDays.Service.Interfaces
{
    public interface IFeriasService : IBaseService<Ferias, FeriasDTO>
    {
        Task<ResultViewModel> CreateFerias(FeriasDTO obj);
        Task<ResultViewModel> UpdateFerias(FeriasDTO obj);
        Task<ResultViewModel> DisableFerias(string id);
        Task<ResultViewModel> GetAllFerias(string? userId);
        Task<ResultViewModel> UpdateStatus(string id, Enumerators.Status status);
    }
}
