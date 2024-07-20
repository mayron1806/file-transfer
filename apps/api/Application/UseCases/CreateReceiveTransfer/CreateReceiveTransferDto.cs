using Domain;

namespace Application.UseCases.CreateReceiveTransfer;

// lista de arquivos para fazer upload
// Name
// ContentType
// Size
public class CreateReceiveTransferInputDto
{
    public int UserId { get; set; }
    public string? Name { get; set; }
    public int OrganizationId { get; set; }
    public DateTime? ExpiresAt { get; set; }
    public string? Password { get; set; }
    public string? Message { get; set; }
    public int? MaxSize { get; set; }
    public int? MaxFiles { get; set; }
    public IEnumerable<string>? AcceptedFiles { get; set; }
}
public class CreateReceiveTransferOutputDto {}
