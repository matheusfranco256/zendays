using ZenDays.Domain.Entities;

namespace ZenDays.Infra.Interfaces
{
	public interface IFeriasRepository : IBaseRepository<Ferias>
	{
		Task<List<Ferias>> GetAllFerias(string? userId, string? status);
		Task<List<Ferias>> GetAllFeriasByDepartamento(string? idDepartamento, string? status);
		Task<List<Ferias>> GetAllFeriasByTipoUsuario(string tipoUsuario, string? idDepartamento, string? idUsuario, string? dataInicio, string? dataFim, string? status);

	}
}
