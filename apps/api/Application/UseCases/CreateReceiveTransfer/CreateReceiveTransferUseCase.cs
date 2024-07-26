using Application.Exceptions;
using Application.Services.PlanService;
using Application.Utils;
using Domain;
using Infrastructure.UnitOfWork;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.CreateReceiveTransfer;

public class CreateReceiveTransferUseCase(
    ILogger<CreateReceiveTransferUseCase> logger,
    IUnitOfWork unitOfWork,
    IPlanService planService
    ) : UseCase<CreateReceiveTransferInputDto, CreateReceiveTransferOutputDto>(logger), ICreateReceiveTransfer
{
    private readonly IPlanService _planService = planService;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    
    public override async Task<CreateReceiveTransferOutputDto> Execute(CreateReceiveTransferInputDto input)
    {
        var organization = await _unitOfWork.Organization.GetByIdAsync(input.OrganizationId);
        if (organization == null) throw new HttpException(400, "Ocorreu um erro ao criar link de recebimento, verifique os dados e tente novamente");
        
        var plan = _planService.GetPlan(organization.Plan ?? "Free");

        #region Validations
        var expiresAt = input.ExpiresAt ?? DateTime.UtcNow.AddDays(plan.Limits.MaxExpireDays);
        if (expiresAt > DateTime.UtcNow.AddDays(plan.Limits.MaxExpireDays)) expiresAt = DateTime.UtcNow.AddDays(plan.Limits.MaxExpireDays);
        
        var maxSize = input.MaxSize ?? (int)plan.Limits.MaxUploadSize;
        if (maxSize > plan.Limits.MaxUploadSize) maxSize = (int)plan.Limits.MaxUploadSize;

        var maxFiles = input.MaxFiles ?? plan.Limits.MaxUploadConcurrency;
        if (maxFiles > plan.Limits.MaxUploadConcurrency) maxFiles = plan.Limits.MaxUploadConcurrency;

        if (input.AcceptedFiles != null && input.AcceptedFiles.Count() > 0) {
            var allIsValid = input.AcceptedFiles.All(FileUtilities.IsAllowedExtension);
            if (!allIsValid) throw new HttpException(400, "Ocorreu um erro ao criar link de recebimento, nem todos os tipos de arquivos selecionados sao permitidos");
        }
        #endregion

        var transfer = new Transfer(
            organizationId: input.OrganizationId,
            transferType: TransferType.Receive,
            expiresAt: expiresAt,
            name: input.Name
        );
        transfer.AddReceive(new Receive(
            message: input.Message,
            password: input.Password != null ? Security.HashPassword(input.Password) : null,
            maxSize: maxSize,
            acceptedFiles: input.AcceptedFiles,
            maxFiles: maxFiles
        ));
        // atualiza organização
        organization.AddTransfer(transfer);
        _unitOfWork.Organization.Update(organization);
        // salva transferencia
        _unitOfWork.Transfer.Add(transfer);
        await _unitOfWork.SaveChangesAsync();
        return new CreateReceiveTransferOutputDto(transfer.Key);
    }
}
