using FluentValidation;

namespace Application.UseCases.CreateReceiveTransfer;

public class CreateReceiveTransferValidator: AbstractValidator<CreateReceiveTransferInputDto>
{
    public CreateReceiveTransferValidator()
    {
        RuleFor(x => x.Password)
            .MaximumLength(100)
            .WithMessage("A senha deve ter no maxímo 100 caracteres");

        RuleFor(x => x.Message)
            .MaximumLength(500)
            .WithMessage("A mensagem deve ter no maximo 500 caracteres");
    }
}