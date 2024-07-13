namespace Domain;

public class Member
{
    public Member(int userId, int organizationId, bool isOwner)
    {
        UserId = userId;
        OrganizationId = organizationId;
        IsOwner = isOwner;
    }
    public Member(int userId, bool isOwner) 
    {
        UserId = userId;
        IsOwner = isOwner;
    }
    public User? User { get; }
    public int UserId { get; }

    public Organization? Organization { get; }
    public int OrganizationId { get; }

    public bool IsOwner { get; }
}
