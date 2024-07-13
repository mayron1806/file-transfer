using FluentValidation;

namespace Application.UseCases.CreateSendTransfer;

public class CreateSendTransferValidator: AbstractValidator<CreateSendTransferInputDto>
{
    public CreateSendTransferValidator()
    {
        RuleForEach(x => x.EmailsDestination)
            .EmailAddress()
            .WithMessage("Email inválido");

        RuleFor(x => x.Password)
            .MaximumLength(100)
            .WithMessage("A senha deve ter no maxímo 100 caracteres");

        RuleFor(x => x.Message)
            .MaximumLength(500)
            .WithMessage("A mensagem deve ter no maximo 500 caracteres");

        RuleForEach(x => x.Files)
            .SetValidator(new CreateSendTransferFileValidator());

        RuleFor(x => x.Files)
            .NotNull().WithMessage("O campo arquivos é obrigatório")
            .Must(x => x?.Any() ?? false).WithMessage("Forneça ao menos 1 arquivo.");

    }
}
class CreateSendTransferFileValidator: AbstractValidator<CreateSendTransferInputDto.FileUpload>
{
    public CreateSendTransferFileValidator()
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