using ZenDays.Core.Models;
using ZenDays.Domain.Entities;
using ZenDays.Service.DTO;

namespace ZenDays.Service.Interfaces
{
    public interface IDepartamentoService : IBaseService<Departamento, DepartamentoDTO>
    {
        Task<ResultViewModel> GetByName(string name);

        Task<ResultViewModel> CreateDepartamento(DepartamentoDTO obj);
        Task<ResultViewModel> UpdateDepartamento(DepartamentoDTO obj);

        Task<ResultViewModel> DisableDepartamento(string id);
    }
}
