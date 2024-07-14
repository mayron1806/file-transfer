namespace Infrastructure.Settings;

public class AWSSettings
{
    public required string AccessKey { get; set; }
    public required string SecretKey { get; set; }
    public required string Endpoint { get; set; }
}
