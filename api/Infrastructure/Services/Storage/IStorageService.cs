namespace Infrastructure.Services.Storage;

public interface IStorageService
{
    Task<IEnumerator<string>> ListBucketsAsync();
    Task<string> GetObjectSignedURLAsync(string bucket, string key);
    Task<string> PutObjectSignedURLAsync(string bucket, string key, string contentType);
    Task<bool> DeleteObjectAsync(string bucket, string key);
    Task<bool> DeleteFolderAsync(string bucket, string key);
    Task<ObjectInfo> GetObjectInfoAsync(string bucket, string key);
}
public class ObjectInfo {
    public ObjectInfo(string contentType, long size, string eTag, DateTime lastModified, string key, string bucketName)
    {
        ContentType = contentType;
        Size = size;
        ETag = eTag;
        LastModified = lastModified;
        Key = key;
        BucketName = bucketName;
    }

    public string ContentType { get; set; }
    public long Size { get; set; }
    public string ETag { get; set; }
    public DateTime LastModified { get; set; }
    public string Key { get; set; }
    public string BucketName { get; set; }
}