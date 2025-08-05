using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using to_do_michelin.Services;
using to_do_michelin.DTOs;

namespace to_do_michelin.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IdentityService _identityService;

        public AuthController(IdentityService identityService)
        {
            _identityService = identityService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO dto)
        {
            if (dto.Password != dto.ConfirmPassword)
                return BadRequest("As senhas não coincidem");

            var result = await _identityService.RegisterAsync(dto);
            
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description);
                return BadRequest(new { errors });
            }

            return Ok(new { message = "Usuário registrado com sucesso" });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO dto)
        {
            var (success, token, error) = await _identityService.LoginAsync(dto);
            
            if (!success)
                return Unauthorized(new { error });

            return Ok(new { token });
        }

        [HttpPost("logout")]
        [Authorize]
        public IActionResult Logout()
        {
            return Ok(new { message = "Logout realizado com sucesso" });
        }

        [HttpGet("profile")]
        [Authorize]
        public async Task<IActionResult> GetProfile()
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var profile = await _identityService.GetUserProfileAsync(userId);
            if (profile == null)
                return NotFound();

            return Ok(profile);
        }

        [HttpPost("change-password")]
        [Authorize]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDTO dto)
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var success = await _identityService.ChangePasswordAsync(userId, dto.CurrentPassword, dto.NewPassword);
            
            if (!success)
                return BadRequest("Não foi possível alterar a senha. Verifique se a senha atual está correta.");

            return Ok(new { message = "Senha alterada com sucesso" });
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDTO dto)
        {
            var success = await _identityService.ResetPasswordAsync(dto.Email, dto.NewPassword);
            
            if (!success)
                return BadRequest("Não foi possível redefinir a senha. Verifique se o email está correto.");

            return Ok(new { message = "Senha redefinida com sucesso" });
        }
    }
}
