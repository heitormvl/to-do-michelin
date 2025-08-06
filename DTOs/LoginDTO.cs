using System.ComponentModel.DataAnnotations;

namespace to_do_michelin.DTOs
{
    public class LoginDTO
    {
        [Required(ErrorMessage = "Username é obrigatório")]
        public required string UserName { get; set; }

        [Required(ErrorMessage = "Senha é obrigatória")]
        public required string Password { get; set; }

        public bool RememberMe { get; set; } = false;
    }
} 