namespace Domain;

public enum FileStatus 
{
    Pending,
    Error,
    Received
}
public class File
{
    public File(string originalName, string key, string path, long size, FileStatus status, string errorMessage, string contentType)
    {
        OriginalName = originalName;
        Key = key;
        Path = path;
        Size = size;
        Status = status;
        ErrorMessage = errorMessage;
        ContentType = contentType;
    }
    public File(string originalName, string key, long size, string contentType, string transferPath, FileStatus status = FileStatus.Pending, string? errorMessage = null)
    {
        OriginalName = originalName;
        Key = key;
        Path = $"{transferPath}/{key}/{originalName}";
        Size = size;
        ContentType = contentType;
        Status = status;
        ErrorMessage = errorMessage;
    }

    public long Id { get; }
    public string Key { get; private set; }
    public string OriginalName { get; private set; }
    public string Path { get; private set; }
    public long Size { get; private set; }
    public FileStatus Status { get; private set; }
    public string? ErrorMessage { get; private set; }
    public string ContentType { get; private set; }
    public Transfer? Transfer { get; private set; }
    public int TransferId { get; private set; }

    public void SetStatus(FileStatus status) => Status = status;
    public void SetErrorMessage(string? errorMessage) => ErrorMessage = errorMessage;
}
