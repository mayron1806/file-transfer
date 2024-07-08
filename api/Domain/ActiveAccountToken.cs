namespace Domain;

public class ActiveAccountToken
{
    // Se necessário, você pode adicionar um construtor adicional para inicialização conveniente
    public ActiveAccountToken(string content, int userId, DateTime expiresAt)
    {
        Content = content;
        UserId = userId;
        ExpiresAt = expiresAt;
        CreatedAt = DateTime.UtcNow;
    }
    public ActiveAccountToken(string content, DateTime expiresAt)
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
    }
}
