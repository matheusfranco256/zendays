using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZenDays.Core.Models;
using ZenDays.Service.DTO;
using ZenDays.Service.Interfaces;

namespace ZenDays.API.Controllers
{
	[Route("api/v1/Usuario")]
	[ApiController]
	// [Authorize(AuthenticationSchemes = "Bearer")]
	public class UsuarioController : BaseController
	{
		private readonly IMapper _mapper;
		private readonly IUserService _userService;
		public UsuarioController(IUserService userService, IMapper mapper)
		{
			_userService = userService;
			_mapper = mapper;
		}
		[HttpPost("Register")]
		public async Task<IActionResult> Register([FromBody] RegisterUserViewModel model) => Result(await _userService.CreateUser(_mapper.Map<UsuarioDTO>(model)));

		[HttpPut("Update")]
		public async Task<IActionResult> Update([FromBody] UpdateUserViewModel model) => Result(await _userService.UpdateUser(_mapper.Map<UsuarioDTO>(model)));


		[HttpGet("GetById")]
		public async Task<IActionResult> Get(string id) => Result(await _userService.Get(id));

		[HttpGet("GetByName")]
		public async Task<IActionResult> GetByName(string name) => Result(await _userService.GetByName(name));

		[HttpGet("GetByEmail")]
		public async Task<IActionResult> GetByEmail(string email) => Result(await _userService.GetByEmail(email));


		[HttpGet("GetAll")]
		public async Task<IActionResult> GetAll() => Result(await _userService.GetAll());

		[HttpGet("GetAllFiltros")]
		public async Task<IActionResult> GetAll(string departamentoId) => Result(await _userService.GetAll(new List<(string field, string value)> { new("IdDepartamento", departamentoId) }));

		[AllowAnonymous]
		[HttpPost()]
		public async Task<IActionResult> VerificaFerias()
		{
			await _userService.VerificaFerias();
			return Result(new ResultViewModel(null, 200, true));
		}

		[HttpDelete("Disable")]
		public async Task<IActionResult> Delete(string id) => Result(await _userService.DisableUser(id));
	}
}
