using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using to_do_michelin.DTOs;
using to_do_michelin.Services;

namespace to_do_michelin.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Realiza o login do usuário
        /// </summary>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO dto)
        {
            var resultado = await _authService.LoginAsync(dto);
            
            if (resultado == null)
                return Unauthorized(new { message = "Credenciais inválidas" });

            return Ok(resultado);
        }

        /// <summary>
        /// Registra um novo usuário
        /// </summary>
        [HttpPost("registro")]
        public async Task<IActionResult> Registro([FromBody] RegistroDTO dto)
        {
            var resultado = await _authService.RegistroAsync(dto);
            
            if (resultado == null)
                return BadRequest(new { message = "Nome de usuário ou email já existe" });

            return Ok(resultado);
        }

        /// <summary>
        /// Solicita redefinição de senha
        /// </summary>
        [HttpPost("redefinir-senha")]
        public async Task<IActionResult> SolicitarRedefinicaoSenha([FromBody] RedefinirSenhaDTO dto)
        {
            var resultado = await _authService.SolicitarRedefinicaoSenhaAsync(dto);
            
            if (!resultado)
                return NotFound(new { message = "Email não encontrado" });

            return Ok(new { message = "Email de redefinição enviado com sucesso" });
        }

        /// <summary>
        /// Define nova senha com token
        /// </summary>
        [HttpPost("nova-senha")]
        public async Task<IActionResult> DefinirNovaSenha([FromBody] NovaSenhaDTO dto)
        {
            var resultado = await _authService.RedefinirSenhaAsync(dto);
            
            if (!resultado)
                return BadRequest(new { message = "Token inválido ou expirado" });

            return Ok(new { message = "Senha alterada com sucesso" });
        }

        /// <summary>
        /// Realiza logout do usuário
        /// </summary>
        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            var username = User.Identity?.Name;
            if (string.IsNullOrEmpty(username))
                return Unauthorized();

            await _authService.LogoutAsync(username);
            return Ok(new { message = "Logout realizado com sucesso" });
        }

        /// <summary>
        /// Obtém informações do usuário logado
        /// </summary>
        [HttpGet("perfil")]
        [Authorize]
        public async Task<IActionResult> ObterPerfil()
        {
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
                return Unauthorized();

            var usuario = await _authService.ObterUsuarioAsync(userId);
            if (usuario == null)
                return NotFound();

            return Ok(usuario);
        }
    }
}
