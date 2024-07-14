using FluentValidation;

namespace Application.UseCases.Login;

public class LoginValidator : AbstractValidator<LoginInputDto>
{
    public LoginValidator()
    {
        RuleFor(x => x.Email).
            NotEmpty().WithMessage("O campo email é obrigatório").
            EmailAddress().WithMessage("O campo email deve ser um email valido");

        RuleFor(x => x.Password).
            NotEmpty().WithMessage("O campo senha é obrigatório");
    }
}
