using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginDTO dto)
    {
        if (dto.Username != "admin" || dto.Password != "123")
            return Unauthorized("Credenciais inválidas.");

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, dto.Username),
            new Claim(ClaimTypes.NameIdentifier, dto.Username)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("chave-super-secreta-bizarra"));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddHours(1),
            signingCredentials: creds);

        return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
    }
}

public class LoginDTO
{
    public string Username { get; set; }
    public string Password { get; set; }
}
