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
    [Authorize(AuthenticationSchemes = "Bearer")]
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
        public async Task<IActionResult> Register([FromBody] RegisterUserViewModel model)
        {
            return Result(await _userService.CreateUser(_mapper.Map<UsuarioDTO>(model)));
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] UpdateUserViewModel model)
        {
            return Result(await _userService.UpdateUser(_mapper.Map<UsuarioDTO>(model)));
        }


        [HttpGet("GetById")]
        public async Task<IActionResult> Get(string id)
        {
            return Result(await _userService.Get(id));
        }

        [HttpGet("GetByName")]
        public async Task<IActionResult> GetByName(string name)
        {
            return Result(await _userService.GetByName(name));
        }
        [HttpGet("GetByEmail")]
        public async Task<IActionResult> GetByEmail(string email)
        {
            return Result(await _userService.GetByEmail(email));
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            return Result(await _userService.GetAll());
        }

        [HttpDelete("Disable")]
        public async Task<IActionResult> Delete(string id)
        {
            return Result(await _userService.DisableUser(id));
        }
    }
}
