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
			obj.SaldoFerias = 0;
			obj.DataCadastro = DateTime.Now.ToString();
			obj.FinalPeriodoAquisitivo = DateTime.Now.AddDays(364).ToString();
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

			return new ResultViewModel(null, 201, true);
		}

		public async Task<ResultViewModel> DisableUser(string id)
		{
			var userOld = await _userRepository.Get(id);
			if (userOld == null) return new ResultViewModel(null, 400, false, ErrorMessages.NotFound);
			var userEmail = await _userRepository.GetByEmail(userOld.Email);

			if (userEmail != null && userEmail.Email == "admin@admin.com") return new ResultViewModel(null, 403, false, ErrorMessages.Forbidden);
			var result = await Get(id);
			if (result.Data == null) return new ResultViewModel(null, 404, false, ErrorMessages.NotFound);
			result.Data.Ativo = false;
			var json = JsonConvert.SerializeObject(result.Data);
			var desativaUsuario = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
			if (desativaUsuario == null) return new ResultViewModel(null, 400, false, ErrorMessages.SerializationFailed);

			await Disable(desativaUsuario, id);
			return new ResultViewModel(null, 200, true);
		}

		public async Task<ResultViewModel> GetByEmail(string email)
		{
			var usuario = await _userRepository.GetByEmail(email);
			return usuario == null ? new ResultViewModel(null, 404, false, ErrorMessages.NotFound) : new ResultViewModel(_mapper.Map<UsuarioDTO>(usuario), 200, true, SuccessMessages.Found);
		}

		public async Task<ResultViewModel> GetByName(string name)
		{
			var usuario = await _userRepository.GetByName(name);
			return usuario == null ? new ResultViewModel(null, 404, false, ErrorMessages.NotFound) : new ResultViewModel(_mapper.Map<UsuarioDTO>(usuario), 200, true, SuccessMessages.Found);
		}

		public async Task<ResultViewModel> UpdateUser(UsuarioDTO obj)
		{
			var userOld = await _userRepository.Get(obj.Id);
			if (userOld == null) return new ResultViewModel(null, 400, false, ErrorMessages.NotFound);
			if (userOld.Email == "admin@admin.com") return new ResultViewModel(null, 403, false, ErrorMessages.Forbidden);

			var userEmail = await _userRepository.GetByEmail(obj.Email);
			if (userEmail != null && userEmail.Id != obj.Id) return new ResultViewModel(null, 404, false, ErrorMessages.EmailAlreadyExists);

			var departamento = await _departamentoService.Get(obj.IdDepartamento);
			if (departamento.Data == null) return new ResultViewModel(null, 404, false, ErrorMessages.NotFound);


			obj.Senha = userOld.Senha;
			obj.TipoUsuario = userOld.TipoUsuario;
			obj.DataCadastro = userOld.DataCadastro;
			obj.UltimaVerificacao = userOld.UltimaVerificacao;
			obj.FinalPeriodoAquisitivo = userOld.FinalPeriodoAquisitivo;
			obj.SaldoFerias = userOld.SaldoFerias;
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

			return new ResultViewModel(null, 201, true);
		}

		public async Task VerificaFerias()
		{
			var usuarios = await _userRepository.GetAll();
			var dataAtual = DateTime.Now;

			foreach (var usuario in usuarios)
			{

				if (usuario.FinalPeriodoAquisitivo is not null)
				{
					var finalPeriodoAquisitivo = DateTime.Parse(usuario.FinalPeriodoAquisitivo);

					if (dataAtual >= finalPeriodoAquisitivo)
					{

						int diasASomar = 31;
						usuario.SaldoFerias += diasASomar;

						usuario.FinalPeriodoAquisitivo = dataAtual.AddYears(1).ToString("dd/MM/yyyy");



						Console.WriteLine($"Usuário {usuario.Nome} - Saldo de férias atualizado para {usuario.SaldoFerias} dias.");
					}
					usuario.UltimaVerificacao = dataAtual.ToString("dd/MM/yyyy");

					var json = JsonConvert.SerializeObject(usuario);
					var atualizaUsuario = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
					if (atualizaUsuario is not null) await _userRepository.Update(atualizaUsuario, usuario.Id);
				}
			}
		}
	}
}
