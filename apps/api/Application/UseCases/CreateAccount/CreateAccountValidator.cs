using FluentValidation;

namespace Application.UseCases.CreateAccount
{
    public class CreateAccountValidator : AbstractValidator<CreateAccountInputDto>
    {
        public CreateAccountValidator()
        {
            RuleFor(x => x.Name).
                NotEmpty().WithMessage("O campo nome é obrigatório").
                MinimumLength(3).WithMessage("O campo nome deve ter pelo menos 3 caracteres").
                MaximumLength(100).WithMessage("O campo nome deve ter no maximo 100 caracteres");

            RuleFor(x => x.Email).
                NotEmpty().WithMessage("O campo email é obrigatório").
                EmailAddress().WithMessage("O campo email deve ser um email valido");

            RuleFor(x => x.Password).
                NotEmpty().WithMessage("O campo senha é obrigatório").
                MinimumLength(6).WithMessage("O campo senha deve ter pelo menos 6 caracteres").
                MaximumLength(50).WithMessage("O campo senha deve ter no maximo 50 caracteres").
                Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).*$").WithMessage("A senha deve ter pelo menos uma letra maiúscula, uma letra minúscula, um número e um caractere especial");
        }
    }
}