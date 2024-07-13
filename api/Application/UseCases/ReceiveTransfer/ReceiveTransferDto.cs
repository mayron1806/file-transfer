namespace Application.UseCases.ReceiveTransfer;

public class ReceiveTransferInputDto
{
    public string? Password { get; set; }
    public required string TransferKey { get; set; }
    public List<FileUpload> Files { get; set; } = [];
    public class FileUpload {
        public required string Name { get; set; }
        public required string ContentType { get; set; }
        public long Size { get; set; }
    }
}
public class ReceiveTransferOutputDto {
    public required List<string> Urls { get; set; }
}
