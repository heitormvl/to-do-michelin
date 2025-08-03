using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using to_do_michelin.DTOs;
using to_do_michelin.Models;

namespace to_do_michelin.Services
{
    public class AuthService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<TokenResponseDTO?> LoginAsync(LoginDTO loginDTO)
        {
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Username == loginDTO.Username && u.Ativo);

            if (usuario == null || !VerificarSenha(loginDTO.Password, usuario.PasswordHash))
                return null;

            return GerarToken(usuario);
        }

        public async Task<TokenResponseDTO?> RegistroAsync(RegistroDTO registroDTO)
        {
            if (await _context.Usuarios.AnyAsync(u => u.Username == registroDTO.Username))
                return null;

            if (await _context.Usuarios.AnyAsync(u => u.Email == registroDTO.Email))
                return null;

            var usuario = new Usuario
            {
                Username = registroDTO.Username,
                Email = registroDTO.Email,
                PasswordHash = HashSenha(registroDTO.Password),
                DataCriacao = DateTime.UtcNow,
                Ativo = true
            };

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return GerarToken(usuario);
        }

        public async Task<bool> SolicitarRedefinicaoSenhaAsync(RedefinirSenhaDTO dto)
        {
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Email == dto.Email && u.Ativo);

            if (usuario == null)
                return false;

            var token = GerarTokenRedefinicao();
            usuario.ResetToken = token;
            usuario.ResetTokenExpiry = DateTime.UtcNow.AddHours(24);

            await _context.SaveChangesAsync();

            // Aqui você implementaria o envio de email
            // Por enquanto, apenas retornamos true
            return true;
        }

        public async Task<bool> RedefinirSenhaAsync(NovaSenhaDTO dto)
        {
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.ResetToken == dto.Token && 
                                        u.ResetTokenExpiry > DateTime.UtcNow);

            if (usuario == null)
                return false;

            usuario.PasswordHash = HashSenha(dto.NovaSenha);
            usuario.ResetToken = null;
            usuario.ResetTokenExpiry = null;

            await _context.SaveChangesAsync();
            return true;
        }

        public Task<bool> LogoutAsync(string username)
        {
            // Em uma implementação mais robusta, você poderia invalidar o token
            // Por enquanto, apenas retornamos true
            return Task.FromResult(true);
        }

        public async Task<UsuarioResponseDTO?> ObterUsuarioAsync(int id)
        {
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Id == id && u.Ativo);

            if (usuario == null)
                return null;

            return new UsuarioResponseDTO
            {
                Id = usuario.Id,
                Username = usuario.Username,
                Email = usuario.Email,
                DataCriacao = usuario.DataCriacao,
                Ativo = usuario.Ativo
            };
        }

        private TokenResponseDTO GerarToken(Usuario usuario)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Name, usuario.Username),
                new Claim(ClaimTypes.Email, usuario.Email)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("chave-super-secreta-bizarra-com-32-caracteres"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiraEm = DateTime.UtcNow.AddHours(1);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: expiraEm,
                signingCredentials: creds);

            return new TokenResponseDTO
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Username = usuario.Username,
                ExpiraEm = expiraEm
            };
        }

        private string HashSenha(string senha)
        {
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(senha));
            return Convert.ToBase64String(hashedBytes);
        }

        private bool VerificarSenha(string senha, string hash)
        {
            return HashSenha(senha) == hash;
        }

        private string GerarTokenRedefinicao()
        {
            var randomBytes = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);
            return Convert.ToBase64String(randomBytes);
        }
    }
} 