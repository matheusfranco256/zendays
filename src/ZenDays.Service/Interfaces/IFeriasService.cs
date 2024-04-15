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
		Task<ResultViewModel> GetAllFerias(string? userId, string? status);
		Task<ResultViewModel> GetAllFeriasByDepartamento(string departamentoId, string? status);
		Task<ResultViewModel> GetAllFeriasByTipoUsuario(string tipoUsuario, string? status);
		Task<ResultViewModel> UpdateStatus(string id, Enumerators.Status status);
	}
}
