using Application.Exceptions;
using Application.Services.PlanService;
using Application.Utils;
using Domain;
using Infrastructure.Services.Storage;
using Infrastructure.UnitOfWork;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.ReceiveTransfer;

public class ReceiveTransferUseCase(
    ILogger<ReceiveTransferUseCase> logger,
    IUnitOfWork unitOfWork,
    IPlanService planService,
    IStorageService storageService
    ) : UseCase<ReceiveTransferInputDto, ReceiveTransferOutputDto>(logger), IReceiveTransfer
{
    private readonly IStorageService _storageService = storageService;
    private readonly IPlanService _planService = planService;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    
    public override async Task<ReceiveTransferOutputDto> Execute(ReceiveTransferInputDto input)
    {
        // validar se ja foi feito upload
        var transfer = await _unitOfWork.Transfer.GetFirstAsync(x => x.Key == input.TransferKey);
        if (transfer == null) throw new HttpException(400, "Transferencia invalida");

        // validar se transferencia expirou
        if (transfer.Expired) throw new HttpException(400, "Transferencia expirada");

        // validar tipo de transferencia
        if (transfer.TransferType == TransferType.Send) throw new HttpException(400, "Transferencia invalida");
        if (transfer.Receive == null) throw new HttpException(400, "Transferencia invalida");

        // verifica se pode receber arquivos
        if (transfer.Receive?.Status != ReceiveStatus.Pending) throw new HttpException(400, "Arquivos ja foram recebidos ou estão sendo processados");

        // validar senha
        if (!string.IsNullOrEmpty(transfer.Receive?.Password))
        {
            var isValid = Security.VerifyPassword(input.Password ?? "", transfer.Receive.Password);
            if (!isValid) throw new HttpException(401, "Senha inválida");
        }

        var plan = await _planService.GetPlanByOrganizationIdAsync(transfer.OrganizationId);
        
        // validar limite de arquivos
        if (input.Files.Count > transfer.Receive?.MaxFiles || input.Files.Count > plan.Limits.MaxUploadConcurrency) 
        {
            throw new HttpException(400, "Quantidade maxima de arquivos por transferencia ultrapassada");
        }
        
        // validar tamanho dos arquivos
        var filesSize = input.Files.Sum(x => x.Size);
        if (filesSize > transfer.Receive?.MaxSize || filesSize > plan.Limits.MaxUploadSize) 
        {
            throw new HttpException(400, "Tamanho maximo de upload do plano ultrapassado.");
        }
        
        // pegar url para upload de arquivos
        var urls = new List<string>();
        foreach (var file in input.Files)
        {
            var key = Guid.NewGuid().ToString();
            var newFileData = new Domain.File(file.Name, key, file.Size, file.ContentType, transfer.Path!);
            var url = await _storageService.PutObjectSignedURLAsync(StorageBuckets.FileTransfer, newFileData.Path, file.ContentType);
            transfer.AddFile(newFileData);
            urls.Add(url);
        }

        // atualizar transferencia
        transfer.Receive!.UpdateStatus(ReceiveStatus.Processing);
        transfer.SetExpiresAt(DateTime.UtcNow.AddDays(plan.Limits.MaxExpireDays));
        _unitOfWork.Transfer.Update(transfer);
        await _unitOfWork.SaveChangesAsync();
        
        return new ReceiveTransferOutputDto() { Urls = urls };
    }
}
