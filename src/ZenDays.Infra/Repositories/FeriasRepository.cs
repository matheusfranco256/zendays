using Microsoft.Extensions.Configuration;
using ZenDays.Domain.Entities;
using ZenDays.Infra.Interfaces;

namespace ZenDays.Infra.Repositories
{
    public class FeriasRepository : BaseRepository<Ferias>, IFeriasRepository
    {
        public FeriasRepository(IConfiguration configuration) : base(configuration)
        {
        }
    }
}
