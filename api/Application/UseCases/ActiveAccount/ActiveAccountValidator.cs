using FluentValidation;

namespace Application.UseCases.ActiveAccount;

public class ActiveAccountValidator : AbstractValidator<ActiveAccountInputDto>
{
    public ActiveAccountValidator()
    {
        RuleFor(x => x.Token).
            NotEmpty().WithMessage("O token é obrigatório").
            Length(36).WithMessage("O token é invalido");
            
    }    
}
