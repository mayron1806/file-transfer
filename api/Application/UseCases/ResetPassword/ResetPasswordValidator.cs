using FluentValidation;

namespace Application.UseCases.ResetPassword;

public class ResetPasswordValidator: AbstractValidator<ResetPasswordInputDto>
{
    public ResetPasswordValidator()
    {
        RuleFor(x => x.Token).
            NotEmpty().WithMessage("O token é obrigatório").
            Length(36).WithMessage("O token é invalido");
        RuleFor(x => x.Password).
            NotEmpty().WithMessage("O campo senha é obrigatório").
            MinimumLength(6).WithMessage("O campo senha deve ter pelo menos 6 caracteres").
            MaximumLength(50).WithMessage("O campo senha deve ter no maximo 50 caracteres").
            Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).*$").WithMessage("A senha deve ter pelo menos uma letra maiúscula, uma letra minúscula, um número e um caractere especial");
    }
}
