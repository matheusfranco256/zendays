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
    public class FeriasService : BaseService<Ferias, FeriasDTO>, IFeriasService
    {
        private readonly IUserService _userService;
        private readonly IFeriasRepository _feriasRepository;
        public FeriasService(IFeriasRepository feriasRepository, IMapper mapper, IUserService userService) : base(feriasRepository, mapper)
        {
            _userService = userService;
            _feriasRepository = feriasRepository;
        }

        public async Task<ResultViewModel> CreateFerias(FeriasDTO obj)
        {
            var usuario = await _userService.Get(obj.IdUsuario);
            if (usuario.Data == null) return new ResultViewModel(null, 404, false, ErrorMessages.NotFound);
            var json = JsonConvert.SerializeObject(obj);
            var insereFerias = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
            if (insereFerias == null) return new ResultViewModel(null, 400, false, ErrorMessages.SerializationFailed);
            return await Create(insereFerias);
        }

        public async Task<ResultViewModel> DisableFerias(string id)
        {
            var result = await Get(id);
            if (result.Data == null) return new ResultViewModel(null, 404, false, ErrorMessages.NotFound);
            result.Data.Ativo = false;
            var json = JsonConvert.SerializeObject(result.Data);
            var desativaFerias = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
            if (desativaFerias == null) return new ResultViewModel(null, 400, false, ErrorMessages.SerializationFailed);
            return await Disable(desativaFerias, id);
        }

        public async Task<ResultViewModel> UpdateFerias(FeriasDTO obj)
        {
            var usuario = await _userService.Get(obj.IdUsuario);
            if (usuario.Data == null) return new ResultViewModel(null, 404, false, ErrorMessages.NotFound);
            var json = JsonConvert.SerializeObject(obj);
            var atualizaUsuario = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
            if (atualizaUsuario == null) return new ResultViewModel(null, 400, false, ErrorMessages.SerializationFailed);
            return await Update(atualizaUsuario, obj.Id);
        }

        public async Task<ResultViewModel> GetAllFerias(string? userId)
        {
            var fromdb = await _feriasRepository.GetAllFerias(userId);
            if (fromdb.Count == 0)
            {
                return new ResultViewModel(null, 404, false, ErrorMessages.NotFound);
            }
            return new ResultViewModel(_mapper.Map<List<FeriasDTO>>(fromdb), 200, true, SuccessMessages.Found);
        }

    }
}
