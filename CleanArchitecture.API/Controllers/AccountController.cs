using CleanArchitecture.Application.Contracts.Identity;
using CleanArchitecture.Application.Models.Identity.Request;
using CleanArchitecture.Application.Models.Identity.Response;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController(IAuthService authService) : ControllerBase
    {
        private readonly IAuthService _authService = authService;

        [HttpPost("Login")]
        public async Task<ActionResult<AuthResponse>> Login([FromBody] AuthRequest command)
        {
            return Ok(await _authService.Login(command));
        }

        [HttpPost("Register")]
        public async Task<ActionResult<RegistrationResponse>> Register([FromBody] RegistrationRequest command)
        {
            return Ok(await _authService.Register(command));
        }
    }
}
