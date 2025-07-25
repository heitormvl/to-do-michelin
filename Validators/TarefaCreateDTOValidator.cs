using FluentValidation;

public class TarefaCreateDTOValidator : AbstractValidator<TarefaCreateDTO>
{
    public TarefaCreateDTOValidator()
    {
        RuleFor(x => x.Titulo)
            .NotEmpty().WithMessage("O título é obrigatório")
            .MinimumLength(3).WithMessage("Mínimo de 3 caracteres");
    }
}
