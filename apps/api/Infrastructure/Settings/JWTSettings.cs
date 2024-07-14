namespace Infrastructure.Settings;

public class JWTSettings
{
    public required string Key { get; set; }
    public string? Issuer { get; set; }
    public string? Audience { get; set; }
    public int ExpiresInMinutes { get; set; }
}