using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using to_do_michelin.Models;
using to_do_michelin.DTOs;

namespace to_do_michelin.Services
{
    public class IdentityService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;

        public IdentityService(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        public async Task<IdentityResult> RegisterAsync(RegisterDTO dto)
        {
            var user = new ApplicationUser
            {
                UserName = dto.UserName,
                Email = dto.Email,
                NomeCompleto = dto.NomeCompleto,
                EmailConfirmed = true // Para simplificar, não vamos confirmar email
            };

            var result = await _userManager.CreateAsync(user, dto.Password);
            
            if (result.Succeeded)
            {
                // Verificar se a role existe antes de adicionar
                if (await _userManager.GetRolesAsync(user) is var roles && !roles.Contains("User"))
                {
                    var roleResult = await _userManager.AddToRoleAsync(user, "User");
                    if (!roleResult.Succeeded)
                    {
                        // Se não conseguir adicionar a role, deletar o usuário criado
                        await _userManager.DeleteAsync(user);
                        return roleResult;
                    }
                }
            }

            return result;
        }

        public async Task<(bool success, string? token, string? error)> LoginAsync(LoginDTO dto)
        {
            var user = await _userManager.FindByNameAsync(dto.UserName);
            if (user == null)
                return (false, null, "Usuário não encontrado");

            if (!user.Ativo)
                return (false, null, "Usuário inativo");

            var result = await _signInManager.CheckPasswordSignInAsync(user, dto.Password, false);
            
            if (!result.Succeeded)
                return (false, null, "Senha incorreta");

            // Atualizar último login
            user.UltimoLogin = DateTime.UtcNow;
            await _userManager.UpdateAsync(user);

            // Gerar token JWT
            var token = await GenerateJwtTokenAsync(user);
            
            return (true, token, null);
        }

        public async Task<UserProfileDTO?> GetUserProfileAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return null;

            var roles = await _userManager.GetRolesAsync(user);

            return new UserProfileDTO
            {
                Id = user.Id,
                UserName = user.UserName ?? string.Empty,
                Email = user.Email ?? string.Empty,
                NomeCompleto = user.NomeCompleto,
                DataCriacao = user.DataCriacao,
                UltimoLogin = user.UltimoLogin,
                Ativo = user.Ativo,
                EmailConfirmed = user.EmailConfirmed,
                Roles = roles.ToList()
            };
        }

        public async Task<bool> ChangePasswordAsync(string userId, string currentPassword, string newPassword)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return false;

            var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
            return result.Succeeded;
        }

        public async Task<bool> ResetPasswordAsync(string email, string newPassword)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) return false;

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, newPassword);
            return result.Succeeded;
        }

        public async Task<bool> DeactivateUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return false;

            user.Ativo = false;
            var result = await _userManager.UpdateAsync(user);
            return result.Succeeded;
        }

        private async Task<string> GenerateJwtTokenAsync(ApplicationUser user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName ?? string.Empty),
                new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
                new Claim("NomeCompleto", user.NomeCompleto ?? string.Empty)
            };

            // Adicionar roles como claims
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("chave-super-secreta-bizarra-com-32-caracteres"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
} 