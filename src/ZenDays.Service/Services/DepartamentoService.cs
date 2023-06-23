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
        public DepartamentoService(IDepartamentoRepository departamentoRepository, IMapper mapper) : base(departamentoRepository, mapper)
        {
            _departamentoRepository = departamentoRepository;
        }

        public async Task<ResultViewModel> CreateDepartamento(DepartamentoDTO obj)
        {
            var depNameExists = await _departamentoRepository.GetByName(obj.Nome);
            if (depNameExists != null) return new ResultViewModel(null, 404, false, ErrorMessages.DepAlreadyExists);
            var json = JsonConvert.SerializeObject(obj);
            var insereDepartamento = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
            if (insereDepartamento == null) return new ResultViewModel(null, 400, false, ErrorMessages.SerializationFailed);
            return await Create(insereDepartamento);
        }

        public async Task<ResultViewModel> DisableDepartamento(string id)
        {
            var result = await Get(id);
            if (result.Data == null) return new ResultViewModel(null, 404, false, ErrorMessages.NotFound);
            result.Data.Ativo = false;
            var json = JsonConvert.SerializeObject(result.Data);
            var desativaDepartamento = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
            if (desativaDepartamento == null) return new ResultViewModel(null, 400, false, ErrorMessages.SerializationFailed);
            return await Disable(desativaDepartamento, id);
        }

        public async Task<ResultViewModel> GetByName(string name)
        {
            var departamento = await _departamentoRepository.GetByName(name);

            if (departamento == null)
            {
                return new ResultViewModel(null, 404, false, ErrorMessages.NotFound);
            }
            return new ResultViewModel(_mapper.Map<DepartamentoDTO>(departamento), 200, true, SuccessMessages.Found);
        }

        public async Task<ResultViewModel> UpdateDepartamento(DepartamentoDTO obj)
        {
            var depNameExists = await _departamentoRepository.GetByName(obj.Nome);
            if (depNameExists != null && depNameExists.Id != obj.Id) return new ResultViewModel(null, 404, false, ErrorMessages.DepAlreadyExists);
            var json = JsonConvert.SerializeObject(obj);
            var atualizaDepartamento = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
            if (atualizaDepartamento == null) return new ResultViewModel(null, 400, false, ErrorMessages.SerializationFailed);
            return await Update(atualizaDepartamento, obj.Id);
        }
    }
}
