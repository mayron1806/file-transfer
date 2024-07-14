using Application.Exceptions;
using Domain;
using Infrastructure.Services.Storage;
using Infrastructure.UnitOfWork;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.ConfirmFileReceive;

public class ConfirmFileReceiveUseCase(
    ILogger<ConfirmFileReceiveUseCase> logger,
    IUnitOfWork unitOfWork,
    IStorageService storageService
    ) : UseCase<ConfirmFileReceiveInputDto, ConfirmFileReceiveOutputDto>(logger), IConfirmFileReceive
{
    private readonly IStorageService _storageService = storageService;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly int FILE_READ_BATCH_SIZE = 500;

    public override async Task<ConfirmFileReceiveOutputDto> Execute(ConfirmFileReceiveInputDto input)
    {
        var transfer = await _unitOfWork.Transfer.GetFirstAsync(x => x.Key == input.TransferKey);
        if (transfer == null) throw new HttpException(400, "Transferencia invalida");
        var count = await _unitOfWork.File.CountByTransferAsync(transfer.Id);
        if(count == 0) throw new HttpException(400, "Nenhum arquivo enviado");
        List<FileErrorDto> errors = [];
        using (var transaction = await _unitOfWork.BeginTransactionAsync())
        {
            try
            {
                for (int i = 0; i < count; i += FILE_READ_BATCH_SIZE) {

                    var files = await _unitOfWork.File.GetByTransferAsync(transfer.Id, FILE_READ_BATCH_SIZE, i);
                    _logger.LogInformation("Transferencia: " + transfer.Id.ToString());
                    _logger.LogInformation("Lendo arquivos: " + i + " - " + (i + FILE_READ_BATCH_SIZE).ToString());
                    _logger.LogInformation("Arquivos: " + files.Count().ToString());
                    var validations = files.Select(ValidateFile).ToList();

                    var res = await Task.WhenAll(validations);
                    // confirmar arquivos enviados
                    foreach (var (fileId, isValid, error) in res)
                    {
                        var file = files.First(x => x.Id == fileId);
                        file.SetStatus(isValid ? FileStatus.Received : FileStatus.Error);
                        file.SetErrorMessage(error);
                        if (!isValid)
                        {
                            await HandleDeleteFile(file.Path);
                            errors.Add(new FileErrorDto {
                                FileId = fileId, 
                                OriginalName = file.OriginalName, 
                                Error = error!
                            });
                        }
                    }
                    _logger.LogInformation($"Confirmados: {res.Count(x => x.IsValid)} - Erros: {res.Count(x => !x.IsValid)}");
                    await _unitOfWork.File.UpdateRangeAsync(files);
                }
                await _unitOfWork.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao confirmar os arquivos");
                await transaction.RollbackAsync();
                throw new HttpException(500, "Ocorreu um erro ao tentar confirmar os arquivos");
            }
        }
        return new ConfirmFileReceiveOutputDto() { FilesWithError = errors };
    }
    private async Task HandleDeleteFile(string key)
    {
        try
        {
            await _storageService.DeleteObjectAsync(StorageBuckets.FileTransfer, key);
        }
        catch (Exception)
        {
            _logger.LogWarning($"Erro ao deletar arquivo: {key}");
        }
    }
    private async Task<(long FileId, bool IsValid, string? Error)> ValidateFile(Domain.File file)
    {
        try
        {
            var data = await _storageService.GetObjectInfoAsync(StorageBuckets.FileTransfer, file.Path);
            if (
                data.ContentType != file.ContentType ||
                data.Size != file.Size ||
                !data.Key.Contains(file.Key)
            ) {
                return (file.Id, false, $"Arquivo invalido: {file.OriginalName}");
            }
            _logger.LogWarning($"Arquivo: {data.Key} - Tamanho: {data.Size} - ETag: {data.ETag} - ContentType: {data.ContentType}");
            return (file.Id, true, null);
        }
        catch (Exception ex)
        {
            return (file.Id, false, $"Arquivo invalido ou não encontrado: {file.OriginalName}. Erro: {ex.Message}");
        }
    }

}
