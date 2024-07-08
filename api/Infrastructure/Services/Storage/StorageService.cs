using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using FileTransfer.Shared.Settings;

namespace FileTransfer.Infrastructure.Services.Storage;

public class StorageService : IStorageService
{
    private readonly IAmazonS3 _s3Client;

    public StorageService(AWSSettings settings)
    {
        var credentials = new BasicAWSCredentials(settings.AccessKey, settings.SecretKey);
        _s3Client = new AmazonS3Client(credentials, new AmazonS3Config{
            ServiceURL = settings.Endpoint,
        });
    }

    public Task<string> GetSignedURLAsync(string fileName, string contentType)
    {
        var request = new GetPreSignedUrlRequest
        {
            BucketName = fileName,
            ContentType = contentType,
            PartNumber = 2,
        };
        return Task.FromResult(_s3Client.GetPreSignedURL(request));
    }

    public async Task<IEnumerator<string>> ListBucketsAsync() {
        var response = await _s3Client.ListBucketsAsync();
        return response.Buckets.Select(x => x.BucketName).GetEnumerator();
    }
}
