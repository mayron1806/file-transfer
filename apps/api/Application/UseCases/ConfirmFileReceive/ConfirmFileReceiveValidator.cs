using FluentValidation;

namespace Application.UseCases.ConfirmFileReceive;

public class ConfirmFileReceiveValidator: AbstractValidator<ConfirmFileReceiveInputDto>
{
    public ConfirmFileReceiveValidator()
    {
        RuleFor(x => x.TransferId)
            .NotEmpty().WithMessage("Transferencia invalida");
    }
}