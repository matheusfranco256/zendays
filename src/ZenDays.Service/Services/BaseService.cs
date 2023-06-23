using AutoMapper;
using ZenDays.Core.Messages;
using ZenDays.Core.Models;
using ZenDays.Domain.Entities;
using ZenDays.Infra.Interfaces;
using ZenDays.Service.DTO;
using ZenDays.Service.Interfaces;

namespace ZenDays.Service.Services
{
    public class BaseService<T, J> : IBaseService<T, J>
        where T : Base
        where J : BaseDTO
    {
        protected readonly IMapper _mapper;
        private readonly IBaseRepository<T> _baseRepository;

        public BaseService(IBaseRepository<T> baseRepository, IMapper mapper)
        {
            _baseRepository = baseRepository;
            _mapper = mapper;
        }

        public async Task<ResultViewModel> Create(Dictionary<string, object> obj)
        {
            try
            {
                var fromdb = await _baseRepository.Create(obj);
                return new ResultViewModel(_mapper.Map<J>(fromdb), 200, true, SuccessMessages.Created);
            }
            catch (Exception ex)
            {
                return new ResultViewModel(null, 400, false, ex.Message);
            }
        }

        public async Task<ResultViewModel> Disable(Dictionary<string, object> obj, string id)
        {
            await _baseRepository.Disable(obj, id);
            return new ResultViewModel(null, 200, true, SuccessMessages.Removed);
        }

        public Task<ResultViewModel> Enable(Dictionary<string, object> obj, string id)
        {
            throw new NotImplementedException();
        }

        public async Task<ResultViewModel> Get(string id)
        {
            var fromdb = await _baseRepository.Get(id);
            if (fromdb is null)
            {
                return new ResultViewModel(null, 404, false, ErrorMessages.NotFound);
            }
            return new ResultViewModel(_mapper.Map<J>(fromdb), 200, true, SuccessMessages.Found);
        }

        public async Task<ResultViewModel> GetAll()
        {
            var fromdb = await _baseRepository.GetAll();
            if (fromdb.Count == 0)
            {
                return new ResultViewModel(null, 404, false, ErrorMessages.NotFound);
            }
            return new ResultViewModel(_mapper.Map<List<T>>(fromdb), 200, true, SuccessMessages.Found);
        }

        public async Task<ResultViewModel> Update(Dictionary<string, object> obj, string id)
        {
            try
            {
                var exists = await _baseRepository.Get(id);
                if (exists is null) return new ResultViewModel(null, 404, false, ErrorMessages.NotFound);
                var fromdb = await _baseRepository.Update(obj, id);
                return new ResultViewModel(_mapper.Map<J>(fromdb), 200, true, SuccessMessages.Updated);
            }
            catch (Exception ex)
            {
                return new ResultViewModel(null, 400, false, ex.Message);
            }
        }
    }
}
