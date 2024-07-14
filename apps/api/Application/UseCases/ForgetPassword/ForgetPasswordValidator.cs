using FluentValidation;

namespace Application.UseCases.ForgetPassword;

public class ForgetPasswordValidator : AbstractValidator<ForgetPasswordInputDto>
{
    public ForgetPasswordValidator()
    {
        RuleFor(x => x.Email).
            NotEmpty().WithMessage("O campo email é obrigatório").
            EmailAddress().WithMessage("O campo email deve ser um email valido");
    }
}
