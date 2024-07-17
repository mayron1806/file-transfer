namespace Domain;

public class User
{
    public User(
        int id,
        string name,
        string email,
        string password,
        DateTime createdAt,
        DateTime updatedAt,
        bool emailVerified
        ) : this(
        name, email, password
    )
    {
        Id = id;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
        EmailVerified = emailVerified;
    }
    public User(string name, string email, string password) {
        Name = name;
        Email = email;
        Password = password;
    }

    public int Id { get; }
    public string Name { get; }
    public string Email { get; }
    public string Password { get; private set; }
    public ActiveAccountToken? ActiveAccountToken { get; private set; }
    public ResetPasswordToken? ResetPasswordToken { get; private set; }
    public IEnumerable<Member>? Members { get; set; }
    public DateTime CreatedAt { get; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; private set; } = DateTime.UtcNow;
    public bool EmailVerified { get; private set; } = false;

    public void VerifyEmail() {
        EmailVerified = true;
        // ActiveAccountToken = null;
        UpdatedAt = DateTime.UtcNow;
    }
    public void UpdatePassword(string password) {
        Password = password;
        UpdatedAt = DateTime.UtcNow;
    }
    public void AddActiveAccountToken(ActiveAccountToken token) {
        ActiveAccountToken = token;
        UpdatedAt = DateTime.UtcNow;
    }
    public void AddResetPasswordToken(ResetPasswordToken token) {
        ResetPasswordToken = token;
        UpdatedAt = DateTime.UtcNow;
    }
}