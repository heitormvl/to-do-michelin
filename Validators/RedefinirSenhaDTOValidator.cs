using FluentValidation;
using to_do_michelin.DTOs;

namespace to_do_michelin.Validators
{
    public class RedefinirSenhaDTOValidator : AbstractValidator<RedefinirSenhaDTO>
    {
        public RedefinirSenhaDTOValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email é obrigatório")
                .EmailAddress().WithMessage("Email deve ser válido");

            RuleFor(x => x.NovaSenha)
                .NotEmpty().WithMessage("Nova senha é obrigatória")
                .MinimumLength(6).WithMessage("Nova senha deve ter no mínimo 6 caracteres")
                .MaximumLength(100).WithMessage("Nova senha deve ter no máximo 100 caracteres");

            RuleFor(x => x.ConfirmarSenha)
                .NotEmpty().WithMessage("Confirmação de senha é obrigatória")
                .Equal(x => x.NovaSenha).WithMessage("As senhas devem coincidir");
        }
    }
} 