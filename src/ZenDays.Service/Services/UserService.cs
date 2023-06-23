using AutoMapper;
using FirebaseAdmin.Auth;
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
    public class UserService : BaseService<Usuario, UsuarioDTO>, IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IDepartamentoService _departamentoService;
        public UserService(IUserRepository userRepository, IMapper mapper, IDepartamentoService departamentoService) : base(userRepository, mapper)
        {
            _userRepository = userRepository;
            _departamentoService = departamentoService;
        }

        public async Task<ResultViewModel> CreateUser(UsuarioDTO obj)
        {
            var userEmail = await _userRepository.GetByEmail(obj.Email);
            if (userEmail != null) return new ResultViewModel(null, 404, false, ErrorMessages.EmailAlreadyExists);
            var departamento = await _departamentoService.Get(obj.IdDepartamento);
            if (departamento.Data == null) return new ResultViewModel(null, 404, false, ErrorMessages.NotFound);

            obj.Senha = HashHelper.HashGeneration(obj.Senha);
            var json = JsonConvert.SerializeObject(obj);
            var insereUsuario = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
            if (insereUsuario == null) return new ResultViewModel(null, 400, false, ErrorMessages.SerializationFailed);
            var usuarioCriado = await Create(insereUsuario);
            if (usuarioCriado.Success)
            {

                FirebaseAuth auth = FirebaseAuth.DefaultInstance;
                UserRecord userRecord = await auth.CreateUserAsync(new UserRecordArgs
                {
                    Email = obj.Email,
                    Password = obj.Senha
                });
            }
            return usuarioCriado;
        }

        public async Task<ResultViewModel> DisableUser(string id)
        {
            var result = await Get(id);
            if (result.Data == null) return new ResultViewModel(null, 404, false, ErrorMessages.NotFound);
            result.Data.Ativo = false;
            var json = JsonConvert.SerializeObject(result.Data);
            var desativaUsuario = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
            if (desativaUsuario == null) return new ResultViewModel(null, 400, false, ErrorMessages.SerializationFailed);
            return await Disable(desativaUsuario, id);
        }

        public async Task<ResultViewModel> GetByEmail(string email)
        {
            var usuario = await _userRepository.GetByEmail(email);

            if (usuario == null)
            {
                return new ResultViewModel(null, 404, false, ErrorMessages.NotFound);
            }
            return new ResultViewModel(_mapper.Map<UsuarioDTO>(usuario), 200, true, SuccessMessages.Found);
        }

        public async Task<ResultViewModel> GetByName(string name)
        {
            var usuario = await _userRepository.GetByName(name);

            if (usuario == null)
            {
                return new ResultViewModel(null, 404, false, ErrorMessages.NotFound);
            }
            return new ResultViewModel(_mapper.Map<UsuarioDTO>(usuario), 200, true, SuccessMessages.Found);
        }

        public async Task<ResultViewModel> UpdateUser(UsuarioDTO obj)
        {
            var userOld = await _userRepository.Get(obj.Id);
            if (userOld == null) return new ResultViewModel(null, 400, false, ErrorMessages.NotFound);
            var userEmail = await _userRepository.GetByEmail(obj.Email);
            if (userEmail != null && userEmail.Id != obj.Id) return new ResultViewModel(null, 404, false, ErrorMessages.EmailAlreadyExists);
            var departamento = await _departamentoService.Get(obj.IdDepartamento);
            if (departamento.Data == null) return new ResultViewModel(null, 404, false, ErrorMessages.NotFound);
            obj.Senha = userOld.Senha;
            obj.TipoUsuario = userOld.TipoUsuario;
            var json = JsonConvert.SerializeObject(obj);
            var atualizaUsuario = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
            if (atualizaUsuario == null) return new ResultViewModel(null, 400, false, ErrorMessages.SerializationFailed);
            var usuarioAtualizado = await Update(atualizaUsuario, obj.Id);
            if (usuarioAtualizado.Success)
            {
                FirebaseAuth auth = FirebaseAuth.DefaultInstance;
                var getUserAuth = await auth.GetUserByEmailAsync(userOld.Email);
                await auth.UpdateUserAsync(new UserRecordArgs
                {
                    Uid = getUserAuth.Uid,
                    Email = obj.Email,
                    Password = obj.Senha
                });
            }

            return usuarioAtualizado;
        }
    }
}
