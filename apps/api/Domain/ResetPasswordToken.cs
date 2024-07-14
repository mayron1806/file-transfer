namespace Domain;

public class ResetPasswordToken
{
    // Se necessário, você pode adicionar um construtor adicional para inicialização conveniente
    public ResetPasswordToken(string content, int userId, DateTime expiresAt)
    {
        Content = content;
        UserId = userId;
        ExpiresAt = expiresAt;
        CreatedAt = DateTime.UtcNow;
    }
    public ResetPasswordToken(string content, DateTime expiresAt)
    {
        Content = content;
        ExpiresAt = expiresAt;
        CreatedAt = DateTime.UtcNow;
    }
    public int Id { get; }
    public string Content { get; private set; }
    public User? User { get; private set; }
    public int UserId { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime ExpiresAt { get; private set; }
    public void AddUser(User user) {
        User = user;
        UserId = user.Id;
        ExpiresAt = DateTime.UtcNow;
    }
}
