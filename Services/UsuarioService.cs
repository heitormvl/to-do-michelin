using to_do_michelin.Models;
using to_do_michelin.DTOs;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace to_do_michelin.Services
{
    public class UsuarioService
    {
        private readonly AppDbContext _context;

        public UsuarioService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Usuario?> BuscarPorUsernameAsync(string username)
        {
            return await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Username == username && u.Ativo);
        }

        public async Task<Usuario?> BuscarPorEmailAsync(string email)
        {
            return await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Email == email && u.Ativo);
        }

        public async Task<Usuario?> BuscarPorIdAsync(Guid id)
        {
            return await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Id == id && u.Ativo);
        }

        public async Task<UsuarioReadDTO?> CriarAsync(UsuarioCreateDTO dto)
        {
            // Verificar se username já existe
            if (await BuscarPorUsernameAsync(dto.Username) != null)
                return null;

            // Verificar se email já existe
            if (await BuscarPorEmailAsync(dto.Email) != null)
                return null;

            var usuario = new Usuario
            {
                Username = dto.Username,
                Email = dto.Email,
                PasswordHash = HashPassword(dto.Password)
            };

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return new UsuarioReadDTO
            {
                Id = usuario.Id,
                Username = usuario.Username,
                Email = usuario.Email,
                DataCriacao = usuario.DataCriacao,
                UltimoLogin = usuario.UltimoLogin,
                Ativo = usuario.Ativo
            };
        }

        public async Task<bool> AtualizarAsync(UsuarioUpdateDTO dto)
        {
            var usuario = await BuscarPorIdAsync(dto.Id);
            if (usuario == null) return false;

            if (!string.IsNullOrEmpty(dto.Username) && dto.Username != usuario.Username)
            {
                if (await BuscarPorUsernameAsync(dto.Username) != null)
                    return false;
                usuario.Username = dto.Username;
            }

            if (!string.IsNullOrEmpty(dto.Email) && dto.Email != usuario.Email)
            {
                if (await BuscarPorEmailAsync(dto.Email) != null)
                    return false;
                usuario.Email = dto.Email;
            }

            if (!string.IsNullOrEmpty(dto.Password))
            {
                usuario.PasswordHash = HashPassword(dto.Password);
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RedefinirSenhaAsync(RedefinirSenhaDTO dto)
        {
            if (dto.NovaSenha != dto.ConfirmarSenha)
                return false;

            var usuario = await BuscarPorEmailAsync(dto.Email);
            if (usuario == null) return false;

            usuario.PasswordHash = HashPassword(dto.NovaSenha);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ValidarCredenciaisAsync(string username, string password)
        {
            var usuario = await BuscarPorUsernameAsync(username);
            if (usuario == null) return false;

            return VerifyPassword(password, usuario.PasswordHash);
        }

        public async Task AtualizarUltimoLoginAsync(string username)
        {
            var usuario = await BuscarPorUsernameAsync(username);
            if (usuario != null)
            {
                usuario.UltimoLogin = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
        }

        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }

        private bool VerifyPassword(string password, string hash)
        {
            return HashPassword(password) == hash;
        }
    }
} 