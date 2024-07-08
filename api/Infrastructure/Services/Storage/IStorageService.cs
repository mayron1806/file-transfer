namespace FileTransfer.Infrastructure.Services.Storage;

public interface IStorageService
{
    Task<IEnumerator<string>> ListBucketsAsync();
    Task<string> GetSignedURLAsync(string fileName, string contentType);
    
}
