using AutoMapper;
using Newtonsoft.Json;
using ZenDays.Core.Messages;
using ZenDays.Core.Models;
using ZenDays.Domain.Entities;
using ZenDays.Infra.Interfaces;
using ZenDays.Service.DTO;
using ZenDays.Service.Interfaces;

namespace ZenDays.Service.Services
{
	public class DepartamentoService : BaseService<Departamento, DepartamentoDTO>, IDepartamentoService
	{
		private readonly IDepartamentoRepository _departamentoRepository;
		private readonly IUserRepository _userRepository;
		public DepartamentoService(IDepartamentoRepository departamentoRepository, IMapper mapper, IUserRepository userRepository) : base(departamentoRepository, mapper)
		{
			_departamentoRepository = departamentoRepository;
			_userRepository = userRepository;
		}

		public async Task<ResultViewModel> CreateDepartamento(DepartamentoDTO obj)
		{
			var depNameExists = await _departamentoRepository.GetByName(obj.Nome);
			if (depNameExists != null) return new ResultViewModel(null, 404, false, ErrorMessages.DepAlreadyExists);
			var json = JsonConvert.SerializeObject(obj);
			var insereDepartamento = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
			if (insereDepartamento == null) return new ResultViewModel(null, 400, false, ErrorMessages.SerializationFailed);
			await Create(insereDepartamento);
			return new ResultViewModel(null, 201, true);
		}

		public async Task<ResultViewModel> DeleteDepartamento(string id)
		{
			var result = await _departamentoRepository.Get(id);
			if (result == null) return new ResultViewModel(null, 404, false, ErrorMessages.NotFound);
			if (result.Nome == "ADM") return new ResultViewModel(null, 403, false, ErrorMessages.Forbidden);
			var vinculoDepartamento = await _userRepository.GetByDepartamento(id);
			if (vinculoDepartamento != null) return new ResultViewModel(null, 403, false, ErrorMessages.Forbidden);

			result.Ativo = false;
			var json = JsonConvert.SerializeObject(result);
			var deletaDepartamemto = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
			if (deletaDepartamemto == null) return new ResultViewModel(null, 400, false, ErrorMessages.SerializationFailed);
			var resultDisable = await Delete(deletaDepartamemto, id);

			return resultDisable.Success ? new ResultViewModel(null, 200, true) : resultDisable;
		}

		public async Task<ResultViewModel> GetByName(string name)
		{
			var departamento = await _departamentoRepository.GetByName(name);
			return departamento == null ? new ResultViewModel(null, 404, false, ErrorMessages.NotFound) : new ResultViewModel(_mapper.Map<DepartamentoDTO>(departamento), 200, true, SuccessMessages.Found);
		}

		public async Task<ResultViewModel> UpdateDepartamento(DepartamentoDTO obj)
		{
			var resultId = await _departamentoRepository.Get(obj.Id);
			if (resultId == null) return new ResultViewModel(null, 404, false, ErrorMessages.NotFound);
			if (resultId.Nome == "ADM") return new ResultViewModel(null, 403, false, ErrorMessages.Forbidden);

			var depNameExists = await _departamentoRepository.GetByName(obj.Nome);
			if (depNameExists != null && depNameExists.Id != obj.Id) return new ResultViewModel(null, 404, false, ErrorMessages.DepAlreadyExists);
			var json = JsonConvert.SerializeObject(obj);
			var atualizaDepartamento = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
			if (atualizaDepartamento == null) return new ResultViewModel(null, 400, false, ErrorMessages.SerializationFailed);

			var result = await Update(atualizaDepartamento, obj.Id);
			return result.Success ? new ResultViewModel(null, 200, true) : result;
		}
	}
}
