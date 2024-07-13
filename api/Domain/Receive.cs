namespace Domain;

public enum ReceiveStatus
{
    Pending,
    PendingProcess,
    Processing,
    Error,
    Received
}
public class Receive
{
    public Receive(ReceiveStatus status, string? message, IEnumerable<string>? acceptedFiles, int maxSize, string? password, int? maxFiles)
    {
        Status = status;
        Message = message;
        AcceptedFiles = acceptedFiles;
        MaxSize = maxSize;
        MaxFiles = maxFiles;
        Password = password;
    }
    public Receive(string? message, IEnumerable<string>? acceptedFiles, int maxSize, string? password, int? maxFiles)
    {
        Message = message;
        AcceptedFiles = acceptedFiles;
        MaxSize = maxSize;
        MaxFiles = maxFiles;
        Password = password;
        Status = ReceiveStatus.Pending;
    }

    public ReceiveStatus Status { get; private set; }
    public string? Message { get; private set; }
    public int? MaxFiles { get; private set; }
    public IEnumerable<string>? AcceptedFiles { get; private set; }
    public int MaxSize { get; private set; }
    public string? Password { get; private set; }
    public void UpdateStatus(ReceiveStatus status) => Status = status;
}
