using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using to_do_michelin.Services;
using to_do_michelin.DTOs;

namespace to_do_michelin.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UsuarioService _usuarioService;

        public AuthController(UsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO dto)
        {
            var isValid = await _usuarioService.ValidarCredenciaisAsync(dto.Username, dto.Password);
            if (!isValid)
                return Unauthorized("Credenciais inválidas.");

            await _usuarioService.AtualizarUltimoLoginAsync(dto.Username);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, dto.Username),
                new Claim(ClaimTypes.NameIdentifier, dto.Username)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("chave-super-secreta-bizarra-com-32-caracteres"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds);

            return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
        }

        [HttpPost("logout")]
        [Authorize]
        public IActionResult Logout()
        {
            // Em uma implementação real, você poderia invalidar o token
            // Por enquanto, apenas retornamos sucesso
            return Ok(new { message = "Logout realizado com sucesso" });
        }

        [HttpPost("redefinir-senha")]
        public async Task<IActionResult> RedefinirSenha([FromBody] RedefinirSenhaDTO dto)
        {
            var success = await _usuarioService.RedefinirSenhaAsync(dto);
            if (!success)
                return BadRequest("Não foi possível redefinir a senha. Verifique se o email está correto e se as senhas coincidem.");

            return Ok(new { message = "Senha redefinida com sucesso" });
        }

        [HttpPost("registrar")]
        public async Task<IActionResult> Registrar([FromBody] UsuarioCreateDTO dto)
        {
            var usuario = await _usuarioService.CriarAsync(dto);
            if (usuario == null)
                return BadRequest("Não foi possível criar o usuário. Verifique se o username ou email já não estão em uso.");

            return CreatedAtAction(nameof(Registrar), usuario);
        }

        [HttpGet("perfil")]
        [Authorize]
        public async Task<IActionResult> ObterPerfil()
        {
            var username = User.Identity?.Name;
            if (string.IsNullOrEmpty(username))
                return Unauthorized();

            var usuario = await _usuarioService.BuscarPorUsernameAsync(username);
            if (usuario == null)
                return NotFound();

            var perfil = new UsuarioReadDTO
            {
                Id = usuario.Id,
                Username = usuario.Username,
                Email = usuario.Email,
                DataCriacao = usuario.DataCriacao,
                UltimoLogin = usuario.UltimoLogin,
                Ativo = usuario.Ativo
            };

            return Ok(perfil);
        }
    }

    public class LoginDTO
    {
        public required string Username { get; set; }
        public required string Password { get; set; }
    }
}
