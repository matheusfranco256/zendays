using ZenDays.Domain.Entities;

namespace ZenDays.Infra.Interfaces
{
    public interface IDepartamentoRepository : IBaseRepository<Departamento>
    {
        Task<Departamento?> GetByName(string name, bool isEnabled = true);
    }
}
