using FluentValidation;
using to_do_michelin.DTOs;

namespace to_do_michelin.Validators
{
    public class TarefaUpdateDTOValidator : AbstractValidator<TarefaUpdateDTO>
    {
        public TarefaUpdateDTOValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("O ID da tarefa é obrigatório");

            RuleFor(x => x.Titulo)
                .MinimumLength(3).When(x => !string.IsNullOrEmpty(x.Titulo))
                .WithMessage("O título deve ter pelo menos 3 caracteres")
                .MaximumLength(200).When(x => !string.IsNullOrEmpty(x.Titulo))
                .WithMessage("O título deve ter no máximo 200 caracteres");

            RuleFor(x => x.Descricao)
                .MaximumLength(1000).When(x => !string.IsNullOrEmpty(x.Descricao))
                .WithMessage("A descrição deve ter no máximo 1000 caracteres");
        }
    }
}
