using Microsoft.AspNetCore.Mvc;
using PlataformaIntegral.API.DTOs.Auth;
using PlataformaIntegral.API.Services;
using PlataformaIntegral.API.Services.Auth;

namespace PlataformaIntegral.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        /// Registro de usuario
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            var result = await _authService.RegisterAsync(dto);

            if (result == null || !result.Success)
                return BadRequest(new AuthResponseDto { Success = false, Message = "Error en registro" });


            return Ok(result);
        }

        /// Login
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var result = await _authService.LoginAsync(dto);

            if (result == null || !result.Success)
                return BadRequest(new AuthResponseDto { Success = false, Message = "Credenciales inválidas" });

            return Ok(result);
        }

        // GET: api/Auth/test
        [HttpGet("test")]
        public IActionResult Test()
        {
            return Ok(new { Message = "AuthController funcionando" });
        }
    }
}
