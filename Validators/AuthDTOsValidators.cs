using FluentValidation;
using to_do_michelin.DTOs;

namespace to_do_michelin.Validators
{
    public class LoginDTOValidator : AbstractValidator<LoginDTO>
    {
        public LoginDTOValidator()
        {
            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("O nome de usuário é obrigatório")
                .Length(3, 50).WithMessage("O nome de usuário deve ter entre 3 e 50 caracteres");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("A senha é obrigatória")
                .MinimumLength(6).WithMessage("A senha deve ter pelo menos 6 caracteres");
        }
    }

    public class RegistroDTOValidator : AbstractValidator<RegistroDTO>
    {
        public RegistroDTOValidator()
        {
            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("O nome de usuário é obrigatório")
                .Length(3, 50).WithMessage("O nome de usuário deve ter entre 3 e 50 caracteres")
                .Matches("^[a-zA-Z0-9_]+$").WithMessage("O nome de usuário deve conter apenas letras, números e underscore");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("O email é obrigatório")
                .EmailAddress().WithMessage("Email inválido");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("A senha é obrigatória")
                .MinimumLength(6).WithMessage("A senha deve ter pelo menos 6 caracteres")
                .Matches("[A-Z]").WithMessage("A senha deve conter pelo menos uma letra maiúscula")
                .Matches("[a-z]").WithMessage("A senha deve conter pelo menos uma letra minúscula")
                .Matches("[0-9]").WithMessage("A senha deve conter pelo menos um número");

            RuleFor(x => x.ConfirmPassword)
                .Equal(x => x.Password).WithMessage("As senhas não coincidem");
        }
    }

    public class RedefinirSenhaDTOValidator : AbstractValidator<RedefinirSenhaDTO>
    {
        public RedefinirSenhaDTOValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("O email é obrigatório")
                .EmailAddress().WithMessage("Email inválido");
        }
    }

    public class NovaSenhaDTOValidator : AbstractValidator<NovaSenhaDTO>
    {
        public NovaSenhaDTOValidator()
        {
            RuleFor(x => x.Token)
                .NotEmpty().WithMessage("O token é obrigatório");

            RuleFor(x => x.NovaSenha)
                .NotEmpty().WithMessage("A nova senha é obrigatória")
                .MinimumLength(6).WithMessage("A senha deve ter pelo menos 6 caracteres")
                .Matches("[A-Z]").WithMessage("A senha deve conter pelo menos uma letra maiúscula")
                .Matches("[a-z]").WithMessage("A senha deve conter pelo menos uma letra minúscula")
                .Matches("[0-9]").WithMessage("A senha deve conter pelo menos um número");

            RuleFor(x => x.ConfirmarSenha)
                .Equal(x => x.NovaSenha).WithMessage("As senhas não coincidem");
        }
    }
} 