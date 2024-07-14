namespace Application.UseCases.Login;

public class LoginInputDto
{
    public required string Email { get; set; }
    public required string Password { get; set; }
}
public class LoginOutputDto
{
    public required string AccessToken { get; set; }
    public required string RefreshToken { get; set; }
    public DateTime Expires { get; set; }
}