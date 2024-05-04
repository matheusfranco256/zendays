using ZenDays.Core.Models;
using ZenDays.Core.Utilities;
using ZenDays.Domain.Entities;
using ZenDays.Service.DTO;
using ZenDays.Service.Models;

namespace ZenDays.Service.Interfaces
{
	public interface IFeriasService : IBaseService<Ferias, FeriasDTO>
	{
		Task<ResultViewModel> CreateFerias(CadastraFeriasInputModel obj);
		Task<ResultViewModel> UpdateFerias(FeriasDTO obj);
		Task<ResultViewModel> DeleteFerias(string id);
		Task<ResultViewModel> GetAllFerias(string? userId, string? status);
		Task<ResultViewModel> GetAllFeriasByTipoUsuario(string? tipoUsuario, string? idDepartamento, string? idUsuario, string? dataInicio, string? dataFim, string? status);
		Task<ResultViewModel> UpdateStatus(string id, Enumerators.Status status);
	}
}
