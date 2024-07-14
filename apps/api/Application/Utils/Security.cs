namespace Application.Utils;

public static class Security
{
    private static readonly int salt = 10;
    public static string HashPassword(string password) => BCrypt.Net.BCrypt.HashPassword(password, salt);

    public static bool VerifyPassword(string password, string hash) => BCrypt.Net.BCrypt.Verify(password, hash);
    
}