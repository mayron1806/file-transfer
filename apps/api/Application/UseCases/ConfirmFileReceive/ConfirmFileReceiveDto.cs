namespace Application.UseCases.ConfirmFileReceive;

public class ConfirmFileReceiveInputDto
{
    public required string TransferKey { get; set; }
}
public class ConfirmFileReceiveOutputDto
{
    public required List<FileErrorDto> FilesWithError { get; set; }
}
public class FileErrorDto
{
    public required long FileId { get; set; }
    public required string OriginalName { get; set; }
    public required string Error { get; set; }
}
