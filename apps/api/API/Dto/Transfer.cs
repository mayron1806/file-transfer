using Domain;

namespace API.Dto;
public class SimpleTransfer(int id, string key, string name, DateTime createdAt, DateTime expiresAt, long size, int filesCount, TransferType transferType, SimpleTransfer.TransferReceiveDto? receive, SimpleTransfer.TransferSendDto? send)
{
    public int Id { get; private set; } = id;
    public string Key { get; private set; } = key;
    public string Name { get; private set; } = name;
    public DateTime ExpiresAt { get; private set; } = expiresAt;
    public DateTime CreatedAt { get; private set; } = createdAt;
    public long Size { get; private set; } = size;
    public int FilesCount { get; private set; } = filesCount;
    public TransferType TransferType { get; private set; } = transferType;
    public TransferReceiveDto? Receive { get; private set; } = receive;
    public TransferSendDto? Send { get; private set; } = send;

    public class TransferReceiveDto(bool hasPassword)
    {
        public bool HasPassword { get; private set; } = hasPassword;
    }
    public class TransferSendDto(bool hasPassword, int downloads, bool expiresOnDownload)
    {
        public bool HasPassword { get; private set; } = hasPassword;
        public int Downloads { get; private set; } = downloads;
        public bool ExpiresOnDownload { get; private set; } = expiresOnDownload;
    }
}
public class TransferGetTransferListDto 
{
    public IEnumerable<SimpleTransfer> Transfers { get; private set; } = [];
    public int TransfersCount { get; set; }
    
    public static TransferGetTransferListDto Map(IEnumerable<Transfer> transfers, int transfersCount)
    {
        var res = new TransferGetTransferListDto
        {
            TransfersCount = transfersCount,
            Transfers = transfers.Select(x => new SimpleTransfer(
            id: x.Id,
            key: x.Key,
            name: string.IsNullOrEmpty(x.Name) ? x.Key : x.Name,
            createdAt: x.CreatedAt,
            expiresAt: x.ExpiresAt,
            size: x.Size,
            filesCount: x.FilesCount,
            transferType: x.TransferType,
            receive: x.TransferType == TransferType.Receive ? new SimpleTransfer.TransferReceiveDto(
                hasPassword: !string.IsNullOrEmpty(x.Receive!.Password)
            ) : null,
            send: x.TransferType == TransferType.Send ?  new SimpleTransfer.TransferSendDto(
                hasPassword: !string.IsNullOrEmpty(x.Send!.Password),
                downloads: x.Send!.Downloads,
                expiresOnDownload: x.Send!.ExpiresOnDowload
            ) : null
        ))
        };
        return res;
    }
}
public class TransferGetTrasferDto(int id, string key, string? name, IEnumerable<TransferGetTrasferDto.TransferFileDto>? files, DateTime createdAt, DateTime expiresAt, long size, int filesCount, string path, TransferType transferType, TransferGetTrasferDto.TransferReceiveDto receive, TransferGetTrasferDto.TransferSendDto send)
{
    public int Id { get; private set; } = id;
    public string Key { get; private set; } = key;
    public string? Name { get; private set; } = name;
    public IEnumerable<TransferFileDto>? Files { get; private set; } = files;
    public DateTime CreatedAt { get; private set; } = createdAt;
    public DateTime ExpiresAt { get; private set; } = expiresAt;
    public long Size { get; private set; } = size;
    public int FilesCount { get; private set; } = filesCount;
    public string Path { get; private set; } = path;
    public TransferType TransferType { get; set; } = transferType;
    public TransferReceiveDto Receive { get; set; } = receive;
    public TransferSendDto Send { get; set; } = send;
    public class TransferFileDto(long id, string key, string originalName, string path, string? errorMessage, long size, string contentType)
    {
        public long Id { get; } = id;
        public string Key { get; private set; } = key;
        public string OriginalName { get; private set; } = originalName;
        public string Path { get; private set; } = path;
        public string? ErrorMessage { get; private set; } = errorMessage;
        public long Size { get; private set; } = size;
        public string ContentType { get; private set; } = contentType;
    }
    public class TransferReceiveDto(string? message, IEnumerable<string>? acceptedFiles, int maxSize, bool hasPassword)
    {
        public bool Received { get; }
        public string? Message { get; } = message;
        public IEnumerable<string>? AcceptedFiles { get; } = acceptedFiles;
        public int MaxSize { get; } = maxSize;
        public bool HasPassword { get; private set; } = hasPassword;
    }
    public class TransferSendDto(string? message, bool hasPassword, int downloads, bool expiresOnDowload, IEnumerable<string>? destination)
    {
        public string? Message { get; private set; } = message;
        public bool HasPassword { get; private set; } = hasPassword;
        public int Downloads { get; private set; } = downloads;
        public bool ExpiresOnDowload { get; private set; } = expiresOnDowload;
        public IEnumerable<string>? Destination { get; private set; } = destination;
    }

    public static TransferGetTrasferDto Map(Transfer transfer)
    {
        var res = new TransferGetTrasferDto
        (
            id: transfer.Id,
            key: transfer.Key,
            name: transfer.Name,
            files: transfer.Files?.Select(x => new TransferFileDto
            (
                id: x.Id,
                key: x.Key,
                originalName: x.OriginalName,
                path: x.Path,
                errorMessage: x.ErrorMessage,
                size: x.Size,
                contentType: x.ContentType
            )),
            createdAt: transfer.CreatedAt,
            expiresAt: transfer.ExpiresAt,
            size: transfer.Size,
            filesCount: transfer.FilesCount,
            path: transfer.Path,
            transferType: transfer.TransferType,
            receive: new TransferReceiveDto
            (
                message: transfer.Receive?.Message,
                acceptedFiles: transfer.Receive?.AcceptedFiles,
                maxSize: transfer.Receive?.MaxSize ?? 0,
                hasPassword: transfer.TransferType == TransferType.Receive && !string.IsNullOrEmpty(transfer.Receive?.Password)
            ),
            send: new TransferSendDto
            (
                message: transfer.Send?.Message,
                hasPassword: transfer.TransferType == TransferType.Send && !string.IsNullOrEmpty(transfer.Receive?.Password),
                downloads: transfer.Send?.Downloads ?? 0,
                expiresOnDowload: transfer.Send?.ExpiresOnDowload ?? false,
                destination: transfer.Send?.Destination
            )
        );
        return res;
    }
}