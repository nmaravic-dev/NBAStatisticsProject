using Microsoft.AspNetCore.Mvc;
using NBAStatisticsProject.DTOs;
using NBAStatisticsProject.Services;

namespace NBAStatisticsProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _service;
        public AuthController(IAuthService service) => _service = service;

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser(RegisterDto dto)
        {
            var result = await _service.RegisterUserAsync(dto);
            if (result == null)
                return BadRequest("Registration failed (email may be taken or password too weak).");
            return Ok(result);
        }
        [HttpPost("login")]
        public async Task<IActionResult> LoginUser(LoginDto dto)
        {
            var result = await _service.LoginUserAsync(dto);
            if (result == null)
                return Unauthorized("Invalid email or password.");
            return Ok(result);
        }
    }
}
