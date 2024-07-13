using Domain;
using Infrastructure.Services.Storage;
using Infrastructure.UnitOfWork;
using Microsoft.Extensions.Logging;
using Quartz;

namespace Application.Jobs;

public class DeleteExpiredFile(IUnitOfWork unitOfWork, IStorageService storageService, ILogger<DeleteExpiredFile> logger) : IJob
{

    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IStorageService _storageService = storageService;
    private readonly ILogger<DeleteExpiredFile> _logger = logger;

    public async Task Execute(IJobExecutionContext context)
    {
        var transfers = await _unitOfWork.Transfer.GetExpiredTransfers();
        var tasks = transfers.Select(DeleteTransfer);
        await Task.WhenAll(tasks);
    }
    private async Task DeleteTransfer(Transfer transfer) {
        try
        {
            await _storageService.DeleteFolderAsync(StorageBuckets.FileTransfer, transfer.Path);
        }
        catch (Amazon.S3.AmazonS3Exception)
        {
            _logger.LogWarning($"Pasta: {transfer.Path} não existe");
        }
        finally
        {
            transfer.SetAsExpired();
            _unitOfWork.Transfer.Update(transfer);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}