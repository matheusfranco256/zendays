using AutoMapper;
using Newtonsoft.Json;
using ZenDays.Core.Messages;
using ZenDays.Core.Models;
using ZenDays.Core.Utilities;
using ZenDays.Domain.Entities;
using ZenDays.Infra.Interfaces;
using ZenDays.Service.DTO;
using ZenDays.Service.Interfaces;

namespace ZenDays.Service.Services
{
	public class FeriasService : BaseService<Ferias, FeriasDTO>, IFeriasService
	{
		private readonly IUserService _userService;
		private readonly IFeriasRepository _feriasRepository;
		private readonly IUserRepository _userRepository;
		public FeriasService(IFeriasRepository feriasRepository, IMapper mapper, IUserService userService, IUserRepository userRepository) : base(feriasRepository, mapper)
		{
			_userService = userService;
			_feriasRepository = feriasRepository;
			_userRepository = userRepository;
		}

		public async Task<ResultViewModel> CreateFerias(FeriasDTO obj)
		{
			var usuario = await _userService.Get(obj.IdUsuario);
			if (usuario.Data == null) return new ResultViewModel(null, 404, false, ErrorMessages.NotFound);


			var diferencaEmMilissegundos = DateTime.Parse(obj.DataInicio) - DateTime.Parse(obj.DataFim);
			var qtdeDias = diferencaEmMilissegundos.TotalDays;

			if (usuario.Data.SaldoFerias < qtdeDias) return new ResultViewModel(null, 400, false, ErrorMessages.BadRequest);
			var json = JsonConvert.SerializeObject(obj);
			var insereFerias = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
			if (insereFerias == null) return new ResultViewModel(null, 400, false, ErrorMessages.SerializationFailed);
			var result = await Create(insereFerias);
			return result.Success ? new ResultViewModel(null, 201, true) : result;
		}
		public async Task<ResultViewModel> DisableFerias(string id)
		{
			var result = await Get(id);
			if (result.Data == null) return new ResultViewModel(null, 404, false, ErrorMessages.NotFound);
			result.Data.Ativo = false;
			var json = JsonConvert.SerializeObject(result.Data);
			var desativaFerias = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
			if (desativaFerias == null) return new ResultViewModel(null, 400, false, ErrorMessages.SerializationFailed);
			await Disable(desativaFerias, id);
			return new ResultViewModel(null, 200, true);
		}
		public async Task<ResultViewModel> UpdateFerias(FeriasDTO obj)
		{
			var usuario = await _userService.Get(obj.IdUsuario);
			if (usuario.Data == null) return new ResultViewModel(null, 404, false, ErrorMessages.NotFound);
			var feriasOld = await _feriasRepository.Get(obj.Id);
			if (feriasOld == null) return new ResultViewModel(null, 404, false, ErrorMessages.NotFound);
			obj.Status = feriasOld.Status;
			var json = JsonConvert.SerializeObject(obj);
			var atualizaFerias = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
			if (atualizaFerias == null) return new ResultViewModel(null, 400, false, ErrorMessages.SerializationFailed);
			var resultUpdate = await Update(atualizaFerias, obj.Id);
			return resultUpdate.Success ? new ResultViewModel(null, 200, true) : resultUpdate;
		}
		public async Task<ResultViewModel> UpdateStatus(string id, Enumerators.Status status)
		{
			var feriasOld = await _feriasRepository.Get(id);
			if (feriasOld == null) return new ResultViewModel(null, 404, false, ErrorMessages.NotFound);
			feriasOld.Status = (int)status;
			var json = JsonConvert.SerializeObject(feriasOld);
			var atualizaFerias = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
			if (atualizaFerias == null) return new ResultViewModel(null, 400, false, ErrorMessages.SerializationFailed);
			if (status == Enumerators.Status.Aprovado)
			{
				var usuario = await _userRepository.Get(feriasOld.IdUsuario);
				if (usuario != null)
				{
					var diferencaEmMilissegundos = DateTime.Parse(feriasOld.DataInicio) - DateTime.Parse(feriasOld.DataFim);
					int qtdeDias = (int)diferencaEmMilissegundos.TotalDays;
					usuario.SaldoFerias -= qtdeDias;

					var jsonUsuario = JsonConvert.SerializeObject(usuario);
					var atualizaUsuario = JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonUsuario);
					if (atualizaUsuario is not null) await _userRepository.Update(atualizaUsuario, usuario.Id);
				}


			}

			var resultUpdate = await Update(atualizaFerias, id);
			return resultUpdate.Success ? new ResultViewModel(null, 200, true) : resultUpdate;

		}
		public async Task<ResultViewModel> GetAllFerias(string? userId)
		{
			var fromdb = await _feriasRepository.GetAllFerias(userId);
			return fromdb.Count == 0 ? new ResultViewModel(null, 404, false, ErrorMessages.NotFound) : new ResultViewModel(_mapper.Map<List<FeriasDTO>>(fromdb), 200, true, SuccessMessages.Found);
		}

	}
}
