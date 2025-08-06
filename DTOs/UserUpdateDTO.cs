using System.ComponentModel.DataAnnotations;

namespace to_do_michelin.DTOs
{
    public class UserUpdateDTO
    {
        [Required(ErrorMessage = "ID do usuário é obrigatório")]
        public required string Id { get; set; }

        [StringLength(50, MinimumLength = 3, ErrorMessage = "Username deve ter entre 3 e 50 caracteres")]
        [RegularExpression(@"^[a-zA-Z0-9_]+$", ErrorMessage = "Username deve conter apenas letras, números e underscore")]
        public string? UserName { get; set; }

        [EmailAddress(ErrorMessage = "Email deve ser válido")]
        [StringLength(100, ErrorMessage = "Email deve ter no máximo 100 caracteres")]
        public string? Email { get; set; }

        [StringLength(100, ErrorMessage = "Nome completo deve ter no máximo 100 caracteres")]
        public string? NomeCompleto { get; set; }

        [StringLength(100, MinimumLength = 6, ErrorMessage = "Senha deve ter entre 6 e 100 caracteres")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).*$", 
            ErrorMessage = "Senha deve conter pelo menos uma letra minúscula, uma maiúscula e um número")]
        public string? Password { get; set; }
    }
}