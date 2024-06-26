﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZenDays.Core.Models;
using ZenDays.Service.DTO;
using ZenDays.Service.Interfaces;

namespace ZenDays.API.Controllers
{
	[Route("api/v1/Departamento")]
	[ApiController]
	[Authorize(AuthenticationSchemes = "Bearer")]
	public class DepartamentoController : BaseController
	{
		private readonly IMapper _mapper;
		private readonly IDepartamentoService _departamentoService;

		public DepartamentoController(IMapper mapper, IDepartamentoService departamentoService)
		{
			_mapper = mapper;
			_departamentoService = departamentoService;
		}

		[HttpPost("Register")]
		public async Task<IActionResult> Register([FromBody] RegisterDepartamentoViewModel model) => Result(await _departamentoService.CreateDepartamento(_mapper.Map<DepartamentoDTO>(model)));

		[HttpPut("Update")]
		public async Task<IActionResult> Update([FromBody] UpdateDepartamentoViewModel model) => Result(await _departamentoService.UpdateDepartamento(_mapper.Map<DepartamentoDTO>(model)));

		[HttpGet("GetById")]
		public async Task<IActionResult> Get(string id) => Result(await _departamentoService.Get(id));

		[HttpGet("GetByName")]
		public async Task<IActionResult> GetByName(string name) => Result(await _departamentoService.GetByName(name));

		[HttpGet("GetAll")]
		public async Task<IActionResult> GetAll() => Result(await _departamentoService.GetAll());

		[HttpDelete("Delete")]
		public async Task<IActionResult> Delete(string id) => Result(await _departamentoService.DeleteDepartamento(id));
	}
}
