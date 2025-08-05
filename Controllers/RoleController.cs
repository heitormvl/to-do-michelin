using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using to_do_michelin.Services;

namespace to_do_michelin.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(Roles = "Admin")]
    public class RoleController : ControllerBase
    {
        private readonly RoleService _roleService;

        public RoleController(RoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpPost("initialize")]
        [AllowAnonymous]
        public async Task<IActionResult> InitializeRoles()
        {
            await _roleService.InitializeRolesAsync();
            return Ok(new { message = "Roles inicializadas com sucesso" });
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRoles()
        {
            var roles = await _roleService.GetAllRolesAsync();
            return Ok(roles);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole([FromBody] string roleName)
        {
            var success = await _roleService.CreateRoleAsync(roleName);
            if (!success)
                return BadRequest("Role já existe ou não foi possível criar");

            return Ok(new { message = $"Role '{roleName}' criada com sucesso" });
        }

        [HttpDelete("{roleName}")]
        public async Task<IActionResult> DeleteRole(string roleName)
        {
            var success = await _roleService.DeleteRoleAsync(roleName);
            if (!success)
                return BadRequest("Role não encontrada ou não foi possível deletar");

            return Ok(new { message = $"Role '{roleName}' deletada com sucesso" });
        }
    }
} 