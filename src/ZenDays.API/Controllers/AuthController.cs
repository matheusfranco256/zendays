using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ZenDays.Core.Models;
using ZenDays.Service.Interfaces;

namespace ZenDays.API.Controllers
{
    [Route("api/v1/Auth")]
    [ApiController]
    public class AuthController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly IAuthService _authService;

        public AuthController(IMapper mapper, IUserService userService, IAuthService authService)
        {
            _mapper = mapper;
            _userService = userService;
            _authService = authService;
        }


        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            return Result(await _authService.AuthenticateUser(model));
        }



        [HttpPut("resetPassword")]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel viewModel)
        {
            return Result(await _authService.ResetPassword(viewModel.Email, viewModel.SenhaAntiga, viewModel.NovaSenha));

        }
    }
}
