using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Infrastructure.Settings;
using Newtonsoft.Json;

namespace Infrastructure.Services.Storage;

public class StorageService : IStorageService
{
    private readonly IAmazonS3 _s3Client;

    public StorageService(AWSSettings settings)
    {
        var credentials = new BasicAWSCredentials(settings.AccessKey, settings.SecretKey);
        _s3Client = new AmazonS3Client(credentials, new AmazonS3Config{ ServiceURL = settings.Endpoint });
        AWSConfigsS3.UseSignatureVersion4 = true;
    }

    public async Task<bool> DeleteObjectAsync(string bucket, string key)
    {
        var request = new DeleteObjectRequest
        {
            BucketName = bucket,
            Key = key
        };
        await _s3Client.DeleteObjectAsync(request);
        return true;
    }
        public async Task<bool> DeleteFolderAsync(string bucket, string key)
    {
        var request = new ListObjectsV2Request
        {
            BucketName = bucket,
            Prefix = key,
        };
        var files = await _s3Client.ListObjectsV2Async(request);
        var delete = new DeleteObjectsRequest 
        {
            BucketName = bucket,
            Objects = files.S3Objects.Select(x => new KeyVersion{Key = x.Key}).ToList()
        };
        await _s3Client.DeleteObjectsAsync(delete);
        return true;
    }

    public async Task<ObjectInfo> GetObjectInfoAsync(string bucket, string key)
    {
        var request = new GetObjectMetadataRequest
        {
            BucketName = bucket,
            Key = key,    
        };
        var res = await _s3Client.GetObjectMetadataAsync(request);
        var obj = new ObjectInfo(res.Headers.ContentType, res.ContentLength, res.ETag, res.LastModified, key, bucket);
        return obj;
    }

    public Task<string> GetObjectSignedURLAsync(string bucket, string key)
    {
        var request = new GetPreSignedUrlRequest
        {
            BucketName = bucket,
            Key = key,
            Expires = DateTime.UtcNow.AddMinutes(5),
        };
        return Task.FromResult(_s3Client.GetPreSignedURL(request));
    }

    public async Task<IEnumerator<string>> ListBucketsAsync() {
        var response = await _s3Client.ListBucketsAsync();
        return response.Buckets.Select(x => x.BucketName).GetEnumerator();
    }

    public Task<string> PutObjectSignedURLAsync(string bucket, string key, string contentType)
    {
        var request = new GetPreSignedUrlRequest
        {
            BucketName = bucket,
            Key = key,
            ContentType = contentType,
            Verb = HttpVerb.PUT,
            Expires = DateTime.UtcNow.AddHours(1),
        };
        return Task.FromResult(_s3Client.GetPreSignedURL(request));
    }
}
