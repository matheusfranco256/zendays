using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZenDays.Core.Models;
using ZenDays.Service.DTO;
using ZenDays.Service.Interfaces;

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
        public async Task<IActionResult> Register([FromBody] RegisterFeriasViewModel model)
        {
            return Result(await _feriasService.CreateFerias(_mapper.Map<FeriasDTO>(model)));
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] UpdateFeriasViewModel model)
        {
            return Result(await _feriasService.UpdateFerias(_mapper.Map<FeriasDTO>(model)));
        }


        [HttpGet("GetById")]
        public async Task<IActionResult> Get(string id)
        {
            return Result(await _feriasService.Get(id));
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            return Result(await _feriasService.GetAll());
        }

        [HttpDelete("Disable")]
        public async Task<IActionResult> Delete(string id)
        {
            return Result(await _feriasService.DisableFerias(id));
        }
    }
}
