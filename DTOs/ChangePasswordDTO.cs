using System.ComponentModel.DataAnnotations;

namespace to_do_michelin.DTOs
{
    public class ChangePasswordDTO
    {
        [Required(ErrorMessage = "Senha atual é obrigatória")]
        public required string CurrentPassword { get; set; }

        [Required(ErrorMessage = "Nova senha é obrigatória")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Senha deve ter entre 6 e 100 caracteres")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).*$", 
            ErrorMessage = "Senha deve conter pelo menos uma letra minúscula, uma maiúscula e um número")]
        public required string NewPassword { get; set; }

        [Required(ErrorMessage = "Confirmação da nova senha é obrigatória")]
        [Compare("NewPassword", ErrorMessage = "As senhas devem coincidir")]
        public required string ConfirmNewPassword { get; set; }
    }
} 