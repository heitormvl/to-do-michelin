using FluentValidation;
using to_do_michelin.DTOs;

namespace to_do_michelin.Validators
{
    public class TarefaCreateDTOValidator : AbstractValidator<TarefaCreateDTO>
    {
        public TarefaCreateDTOValidator()
        {
            RuleFor(x => x.Titulo)
                .NotEmpty().WithMessage("O título é obrigatório")
                .MinimumLength(3).WithMessage("Mínimo de 3 caracteres");
        }
    }
}
