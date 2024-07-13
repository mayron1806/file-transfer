using Domain;

namespace Application.UseCases.CreateSendTransfer;

// lista de arquivos para fazer upload
// Name
// ContentType
// Size
public class CreateSendTransferInputDto
{
    public int OrganizationId { get; set; }
    public DateTime? ExpiresAt { get; set; }
    public IEnumerable<FileUpload> Files { get; set; } = [];
    public string? Password { get; set; }
    public string? Message { get; set; }
    public bool QuickDownload { get; set; }
    public bool ExpiresOnDownload { get; set; }
    public IEnumerable<string>? EmailsDestination { get; set; }
    public class FileUpload {

        public required string Name { get; set; }
        public required string ContentType { get; set; }
        public long Size { get; set; }
    }
}
public class CreateSendTransferOutputDto(IEnumerable<string> urls, Transfer transfer)
{
    public IEnumerable<string> Urls { get; set; } = urls;
    public Transfer Transfer { get; set; } = transfer;
}
