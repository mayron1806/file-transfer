using FluentValidation;

namespace Application.UseCases.ReceiveTransfer;

public class ReceiveTransferValidator: AbstractValidator<ReceiveTransferInputDto>
{
    public ReceiveTransferValidator()
    {
        RuleFor(x => x.Password)
            .MaximumLength(100)
            .WithMessage("A senha deve ter no maxímo 100 caracteres");
        
        RuleForEach(x => x.Files)
            .SetValidator(new ReceiveTransferFileValidator());

        RuleFor(x => x.Files)
            .NotNull().WithMessage("O campo arquivos é obrigatório")
            .Must(x => x?.Any() ?? false).WithMessage("Forneça ao menos 1 arquivo.");
    }
}
class ReceiveTransferFileValidator: AbstractValidator<ReceiveTransferInputDto.FileUpload>
{
    public ReceiveTransferFileValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("O campo nome é obrigatório")
            .Matches(@"^(?!.*[\\/:*?""<>|])(?!^\.)[^\\/:*?""<>|]{1,255}(?<!\.)$")
            .WithMessage("Nome de arquivo inválido");

        RuleFor(x => x.ContentType)
            .NotEmpty()
            .WithMessage("O campo tipo é obrigatório");

        RuleFor(x => x.Size)
            .NotEmpty()
            .WithMessage("O campo tamanho é obrigatório");
    }
}