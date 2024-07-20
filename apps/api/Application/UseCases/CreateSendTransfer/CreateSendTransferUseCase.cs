using Application.Exceptions;
using Application.Services.PlanService;
using Application.Utils;
using Domain;
using Infrastructure.Services.Storage;
using Infrastructure.UnitOfWork;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Application.UseCases.CreateSendTransfer;

public class CreateSendTransferUseCase(
    ILogger<CreateSendTransferUseCase> logger,
    IUnitOfWork unitOfWork,
    IPlanService planService,
    IStorageService storageService
    ) : UseCase<CreateSendTransferInputDto, CreateSendTransferOutputDto>(logger), ICreateSendTransfer
{
    private readonly IStorageService _storageService = storageService;
    private readonly IPlanService _planService = planService;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    
    public override async Task<CreateSendTransferOutputDto> Execute(CreateSendTransferInputDto input)
    {
        if (input.Files == null || !input.Files.Any()) throw new HttpException(400, "Forneça ao menos 1 arquivo");
        
        foreach (var file in input.Files)
        {
            // verificar extensão e mimetype
            var extension = FileUtilities.GetExtension(file.Name);
            var mimeType = file.ContentType;
            if (!FileUtilities.IsAllowedFile(extension, mimeType)) throw new HttpException(400, "Arquivo invalido: " + file.Name);
        }
        
        var organization = await _unitOfWork.Organization.GetByIdAsync(input.OrganizationId);
        if (organization == null) throw new HttpException(400, "Ocorreu um erro ao criar os arquivos, tente mais tarde");
        var plan = _planService.GetPlan(organization.Plan ?? "Free");
        // verifica emails
        if (input.EmailsDestination?.Count() > plan.Limits.MaxEmails) 
        {
            throw new HttpException(400, $"Voce só pode enviar arquivos para até {plan.Limits.MaxEmails} e-mails");
        }

        // verificar tamanho maximo de arquivos
        var filesSize = input.Files.Sum(x => x.Size);
        if (filesSize > plan.Limits.MaxUploadSize) throw new HttpException(400, "Tamanho maximo de upload do plano ultrapassado.");
        
        // verificar qtd upload simultanea
        if (input.Files.Count() > plan.Limits.MaxUploadConcurrency) throw new HttpException(400, "Quantidade maxima de arquivos por transferencia ultrapassada");

        // verificar quantidade maxima de upload diario
        if (organization.DayUploadCount + input.Files.Count() > plan.Limits.MaxUploadPerDay) 
        {
            throw new HttpException(400, "Quantidade maxima de arquivos enviados por dia foi ultrapassada");
        }
        // verificar tamanho maximo de armazenamento da organização
        if (organization.StoredSize + filesSize > plan.Limits.MaxStorageSize) 
        {
            throw new HttpException(400, "O(s) arquivo(s) ultrapassam seu limite de arquivos armazenados para transferencia");
        }
        
        var expiresAt = input.ExpiresAt ?? DateTime.UtcNow.AddDays(plan.Limits.MaxExpireDays);
        if (expiresAt > DateTime.UtcNow.AddDays(plan.Limits.MaxExpireDays)) {
            expiresAt = DateTime.UtcNow.AddDays(plan.Limits.MaxExpireDays);
        }
        var transfer = new Transfer(organization.Id, input.Name, expiresAt, TransferType.Send, filesSize);
        transfer.AddSend(new Send(
            message: input.Message,
            password: plan.Limits.CanUsePassword && !string.IsNullOrEmpty(input.Password) ? Security.HashPassword(input.Password) : null,
            expiresOnDowload: plan.Limits.CanUseExpiresOnDownload && input.ExpiresOnDownload,
            destination: input.EmailsDestination
        ));
        
        // cria urls para os arquivos
        var urls = new List<string>();
        foreach (var file in input.Files)
        {
            var key = Guid.NewGuid().ToString();
            var newFileData = new Domain.File(file.Name, key, file.Size, file.ContentType, transfer.Path!);
            var url = await _storageService.PutObjectSignedURLAsync(StorageBuckets.FileTransfer, newFileData.Path, file.ContentType);
            transfer.AddFile(newFileData);
            urls.Add(url);
        }
        // atualiza organização
        organization.AddTransfer(transfer);
        _unitOfWork.Organization.Update(organization);
        // salva transferencia
        _unitOfWork.Transfer.Add(transfer);
        await _unitOfWork.SaveChangesAsync();
        _logger.LogInformation(JsonConvert.SerializeObject(urls));
        // retorna urls dos arquivos
        return new CreateSendTransferOutputDto(urls, transfer.Id);
    }
}
