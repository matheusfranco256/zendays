using ZenDays.Domain.Entities;

namespace ZenDays.Infra.Interfaces
{
	public interface IFeriasRepository : IBaseRepository<Ferias>
	{
		Task<List<Ferias>> GetAllFerias(string? userId, string? tipoUsuario, string? idDepartamento, string? idUsuarioExcluir, string? status, string? tipoUsuarioExcluir);

	}
}
