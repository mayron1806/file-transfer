namespace Application.UseCases.ActiveAccount
{
    public class ActiveAccountInputDto
    {
        public required string Token { get; set; }
    }
    public class ActiveAccountOutputDto
    {
        public required string AccessToken { get; set; }
        public required string RefreshToken { get; set; }
        public DateTime Expires { get; set; }
    }
}