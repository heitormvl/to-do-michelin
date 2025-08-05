using FluentValidation;
using to_do_michelin.DTOs;

namespace to_do_michelin.Validators
{
    public class RegisterDTOValidator : AbstractValidator<RegisterDTO>
    {
        public RegisterDTOValidator()
        {
            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("Username é obrigatório")
                .Length(3, 50).WithMessage("Username deve ter entre 3 e 50 caracteres")
                .Matches("^[a-zA-Z0-9_]+$").WithMessage("Username deve conter apenas letras, números e underscore");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email é obrigatório")
                .EmailAddress().WithMessage("Email deve ser válido")
                .MaximumLength(100).WithMessage("Email deve ter no máximo 100 caracteres");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Senha é obrigatória")
                .MinimumLength(6).WithMessage("Senha deve ter no mínimo 6 caracteres")
                .MaximumLength(100).WithMessage("Senha deve ter no máximo 100 caracteres")
                .Matches("[A-Z]").WithMessage("Senha deve conter pelo menos uma letra maiúscula")
                .Matches("[a-z]").WithMessage("Senha deve conter pelo menos uma letra minúscula")
                .Matches("[0-9]").WithMessage("Senha deve conter pelo menos um número");

            RuleFor(x => x.ConfirmPassword)
                .Equal(x => x.Password).WithMessage("As senhas devem coincidir");

            RuleFor(x => x.NomeCompleto)
                .MaximumLength(100).WithMessage("Nome completo deve ter no máximo 100 caracteres");
        }
    }
} 