using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZenDays.Core.Models;
using ZenDays.Core.Utilities;
using ZenDays.Service.DTO;
using ZenDays.Service.Interfaces;
using ZenDays.Service.Models;

namespace ZenDays.API.Controllers
{
	[Route("api/v1/Ferias")]
	[ApiController]
	[Authorize(AuthenticationSchemes = "Bearer")]
	public class FeriasController : BaseController
	{
		private readonly IMapper _mapper;
		private readonly IFeriasService _feriasService;

		public FeriasController(IMapper mapper, IFeriasService feriasService)
		{
			_mapper = mapper;
			_feriasService = feriasService;
		}

		[HttpPost("Register")]
		public async Task<IActionResult> Register([FromBody] CadastraFeriasInputModel model) => Result(await _feriasService.CreateFerias(model));

		[HttpPut("Update")]
		public async Task<IActionResult> Update([FromBody] UpdateFeriasViewModel model) => Result(await _feriasService.UpdateFerias(_mapper.Map<FeriasDTO>(model)));

		[HttpGet("GetById")]
		public async Task<IActionResult> Get(string id) => Result(await _feriasService.Get(id));

		[HttpGet]
		[Route("usuario")]
		public async Task<IActionResult> GetAll([FromQuery] string? userId, string? status) => Result(await _feriasService.GetAllFerias(userId, status));
		[HttpGet]
		[Route("departamento")]
		public async Task<IActionResult> GetAllByDepartamento([FromQuery] string? idDepartamento, string? status) => Result(await _feriasService.GetAllFeriasByDepartamento(idDepartamento, status));
		[HttpGet]
		[Route("tipoUsuario")]
		public async Task<IActionResult> GetAllByTipoUsuario([FromQuery][Required] string tipoUsuario, string? status) => Result(await _feriasService.GetAllFeriasByTipoUsuario(tipoUsuario, status));

		[HttpDelete("Disable")]
		public async Task<IActionResult> Delete(string id) => Result(await _feriasService.DisableFerias(id));

		[HttpPatch("Status")]
		public async Task<IActionResult> Status(string id, Enumerators.Status status) => Result(await _feriasService.UpdateStatus(id, status));

	}
}
