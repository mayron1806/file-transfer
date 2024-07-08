namespace Application.UseCases.ResetPassword;

public class ResetPasswordInputDto
{
    public required string Token { get; set; }
    public required string Password { get; set; }
}