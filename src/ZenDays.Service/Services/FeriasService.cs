using AutoMapper;
using Newtonsoft.Json;
using System.Globalization;
using ZenDays.Core.Messages;
using ZenDays.Core.Models;
using ZenDays.Core.Utilities;
using ZenDays.Domain.Entities;
using ZenDays.Infra.Interfaces;
using ZenDays.Service.DTO;
using ZenDays.Service.Interfaces;
using ZenDays.Service.Models;

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

		public async Task<ResultViewModel> CreateFerias(CadastraFeriasInputModel obj)
		{
			var usuario = await _userService.Get(obj.IdUsuario);
			if (usuario.Data == null) return new ResultViewModel(null, 404, false, ErrorMessages.NotFound);


			var diferenca = DateTime.Parse(obj.DataFim) - DateTime.Parse(obj.DataInicio);
			var qtdeDias = diferenca.TotalDays;

			if (usuario.Data.SaldoFerias < qtdeDias) return new ResultViewModel(null, 400, false, "Saldo de ferias insuficiente.");

			var diferencaInicio = DateTime.Parse(obj.DataInicio) - DateTime.Now;
			if (diferencaInicio.TotalDays < 31) return new ResultViewModel(null, 400, false, "um mês de antecedência da data em que quer começar as férias.");

			var dto = new FeriasDTO
			{
				Ativo = true,
				DataInicio = obj.DataInicio,
				DataFim = obj.DataFim,
				DataPedido = DateTime.Now.ToString("d"),
				DataValidacao = "",
				DiasVendidos = obj.DiasVendidos,
				IdUsuario = obj.IdUsuario,
				Mensagem = obj.Mensagem,
				Status = 0
			};

			var json = JsonConvert.SerializeObject(dto);
			var insereFerias = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
			if (insereFerias == null) return new ResultViewModel(null, 400, false, ErrorMessages.SerializationFailed);
			var result = await Create(insereFerias);
			return result.Success ? new ResultViewModel(null, 201, true) : result;
		}
		public async Task<ResultViewModel> DeleteFerias(string id)
		{
			var result = await Get(id);
			if (result.Data == null) return new ResultViewModel(null, 404, false, ErrorMessages.NotFound);
			result.Data.Ativo = false;
			var json = JsonConvert.SerializeObject(result.Data);
			var deletaFerias = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
			if (deletaFerias == null) return new ResultViewModel(null, 400, false, ErrorMessages.SerializationFailed);
			await Delete(deletaFerias, id);
			return new ResultViewModel(null, 200, true);
		}
		public async Task<ResultViewModel> UpdateFerias(FeriasDTO obj)
		{
			var usuario = await _userService.Get(obj.IdUsuario);
			if (usuario.Data == null) return new ResultViewModel(null, 404, false, ErrorMessages.NotFound);
			var feriasOld = await _feriasRepository.Get(obj.Id);
			if (feriasOld == null) return new ResultViewModel(null, 404, false, ErrorMessages.NotFound);
			obj.DataValidacao = feriasOld.DataValidacao;
			obj.DataPedido = feriasOld.DataPedido;
			obj.IdUsuario = feriasOld.IdUsuario;
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
					var diferencaEmMilissegundos = Util.ConvertToDateTime(feriasOld.DataInicio,"/") - Util.ConvertToDateTime(feriasOld.DataFim,"/");
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

     
      
        public async Task<ResultViewModel> GetAllFerias(string? userId, string? status)
		{
			var fromdb = await _feriasRepository.GetAllFerias(userId, status);
			return fromdb.Count == 0 ? new ResultViewModel(null, 404, false, ErrorMessages.NotFound) : new ResultViewModel(_mapper.Map<List<FeriasDTO>>(fromdb), 200, true, SuccessMessages.Found);
		}
	
		public async Task<ResultViewModel> GetAllFeriasByTipoUsuario(string? tipoUsuario,string? idDepartamento,string? idUsuario,string? dataInicio,string? dataFim, string? status)
		{
			var fromdb = await _feriasRepository.GetAllFeriasByTipoUsuario(tipoUsuario, idDepartamento, idUsuario, status);
			if(!string.IsNullOrEmpty(dataInicio) && !string.IsNullOrEmpty(dataFim))
			{
				fromdb = fromdb.Where(x=> Util.ConvertToDateTime(x.DataPedido,"/").Date >= Util.ConvertToDateTime(dataInicio,"/").Date && Util.ConvertToDateTime(x.DataPedido, "/").Date <= Util.ConvertToDateTime(dataFim, "/").Date).ToList();
			}
			return fromdb.Count == 0 ? new ResultViewModel(null, 404, false, ErrorMessages.NotFound) : new ResultViewModel(_mapper.Map<List<FeriasDTO>>(fromdb), 200, true, SuccessMessages.Found);
		}


	}
}
