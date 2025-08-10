using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using to_do_michelin.DTOs;
using to_do_michelin.Models;

namespace to_do_michelin.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(Roles = "Admin")]
    public class UserController : BaseController
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] UserCreateDTO dto)
        {
            var user = new ApplicationUser
            {
                UserName = dto.UserName,
                Email = dto.Email,
                NomeCompleto = dto.NomeCompleto,
                EmailConfirmed = true
            };
            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
                return BadRequest("Erro ao criar usuário", result.Errors.Select(e => e.Description).ToList());
            await _userManager.AddToRoleAsync(user, "User");
            var userDto = new UserReadDTO { Id = user.Id, UserName = user.UserName ?? "", Email = user.Email ?? "", NomeCompleto = user.NomeCompleto };
            return Created(userDto, "Usuário criado com sucesso");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] UserUpdateDTO dto)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound("Usuário não encontrado");
            if (!string.IsNullOrEmpty(dto.UserName)) user.UserName = dto.UserName;
            if (!string.IsNullOrEmpty(dto.Email)) user.Email = dto.Email;
            if (!string.IsNullOrEmpty(dto.NomeCompleto)) user.NomeCompleto = dto.NomeCompleto;
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded) return BadRequest("Erro ao atualizar usuário", result.Errors.Select(e => e.Description).ToList());
            if (!string.IsNullOrEmpty(dto.Password))
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var passResult = await _userManager.ResetPasswordAsync(user, token, dto.Password);
                if (!passResult.Succeeded) return BadRequest("Erro ao alterar senha", passResult.Errors.Select(e => e.Description).ToList());
            }
            var userDto = new UserReadDTO { Id = user.Id, UserName = user.UserName ?? "", Email = user.Email ?? "", NomeCompleto = user.NomeCompleto };
            return Success(userDto, "Usuário atualizado com sucesso");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound("Usuário não encontrado");
            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded) return BadRequest("Erro ao excluir usuário", result.Errors.Select(e => e.Description).ToList());
            return Success("Usuário excluído com sucesso");
        }
    }
}