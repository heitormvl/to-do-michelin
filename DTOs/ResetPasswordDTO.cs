using System.ComponentModel.DataAnnotations;

namespace to_do_michelin.DTOs
{
    public class ResetPasswordDTO
    {
        [Required(ErrorMessage = "Email é obrigatório")]
        [EmailAddress(ErrorMessage = "Email deve ser válido")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "Nova senha é obrigatória")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Senha deve ter entre 6 e 100 caracteres")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).*$", 
            ErrorMessage = "Senha deve conter pelo menos uma letra minúscula, uma maiúscula e um número")]
        public required string NewPassword { get; set; }
    }
} 