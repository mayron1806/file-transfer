using Domain;

namespace API.Dto;

public class LinkGetTrasferDto(int id, string key, IEnumerable<FileDto>? files, DateTime expiresAt, long size, int filesCount, string? message, bool hasPassword)
{
    public int Id { get; private set; } = id;
    public string Key { get; private set; } = key;
    public IEnumerable<FileDto>? Files { get; private set; } = files;
    public DateTime ExpiresAt { get; private set; } = expiresAt;
    public long Size { get; private set; } = size;
    public int FilesCount { get; private set; } = filesCount;
    public string? Message { get; private set; } = message;
    public bool HasPassword { get; } = hasPassword;
    public static LinkGetTrasferDto Map(Transfer transfer)
    {
        var res = new LinkGetTrasferDto(
            id: transfer.Id,
            key: transfer.Key,
            files: transfer.Files?.Select(x => new FileDto(x.Id, x.Key, x.OriginalName, x.Size, x.ContentType)),
            expiresAt: transfer.ExpiresAt,
            size: transfer.Size,
            filesCount: transfer.FilesCount,
            message: transfer.Send?.Message,
            hasPassword: transfer.Send?.Password != null
        );
        return res;
    }
}
public class FileDto(long id, string key, string originalName, long size, string contentType)
{
    public long Id { get; } = id;
    public string Key { get; private set; } = key;
    public string OriginalName { get; private set; } = originalName;
    public long Size { get; private set; } = size;
    public string ContentType { get; private set; } = contentType;
}

public class LinkGetTrasferFilesDto(IEnumerable<FileDto>? files)
{
    public IEnumerable<FileDto>? Files { get; private set; } = files;

    public static LinkGetTrasferFilesDto Map(IEnumerable<Domain.File> files)
    {
        var res = new LinkGetTrasferFilesDto(
            files: files.Select(x => new FileDto(x.Id, x.Key, x.OriginalName, x.Size, x.ContentType))
        );
        return res;
    }
}