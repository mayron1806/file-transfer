using FluentValidation;

namespace Application.UseCases.ConfirmFileReceive;

public class ConfirmFileReceiveValidator: AbstractValidator<ConfirmFileReceiveInputDto>
{
    public ConfirmFileReceiveValidator()
    {
        RuleForEach(x => x.TransferKey)
            .NotEmpty().WithMessage("Transferencia invalida");
    }
}