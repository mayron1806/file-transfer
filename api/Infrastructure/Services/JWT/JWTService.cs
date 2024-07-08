using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Infrastructure.Settings;

namespace Infrastructure.Services.JWT;
public class JWTToken(string accessToken, string refreshToken, DateTime expires) {
    public string AccessToken { get; } = accessToken; 
    public string RefreshToken { get; } = refreshToken;
    public DateTime Expires { get; } = expires;
}
public class JWTService(JWTSettings jwtSettings)
{
    private readonly JWTSettings _jwtSettings = jwtSettings;

    public JWTToken GenerateToken(int userId, string email) {
        return new JWTToken(
            GenerateAccessToken(userId, email), 
            GenerateRefreshToken(userId, email), 
            DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiresInMinutes)
        );
    }
    public string GenerateCustomToken(string content, int? durationInMinutes = null, string? audience = null)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_jwtSettings.Key);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(
            [
                new Claim(ClaimTypes.Name, content),
            ]),
            // Issuer = _jwtSettings.Issuer,
            // Audience = audience ?? "",
            // IssuedAt = DateTime.UtcNow,
            Expires = DateTime.UtcNow.AddMinutes(durationInMinutes ?? 5),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
    private string GenerateAccessToken(int userId, string email)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_jwtSettings.Key);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(
            [
                new Claim(ClaimTypes.Name, userId.ToString()),
                new Claim(ClaimTypes.Email, email)
            ]),
            Issuer = _jwtSettings.Issuer,
            Audience = _jwtSettings.Audience,
            IssuedAt = DateTime.UtcNow,
            Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiresInMinutes),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
    private string GenerateRefreshToken(int userId, string email)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_jwtSettings.Key);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(
            [
                new Claim(ClaimTypes.Name, userId.ToString()),
                new Claim(ClaimTypes.Email, email)
            ]),
            Issuer = _jwtSettings.Issuer,
            IssuedAt = DateTime.UtcNow,
            Audience = $"{_jwtSettings.Audience}/user/refresh",
            Expires = DateTime.UtcNow.AddDays(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
