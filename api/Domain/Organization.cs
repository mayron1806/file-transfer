namespace Domain;

public class Organization
{
    public Organization(string plan, bool planActive)
    {
        Plan = plan;
        PlanActive = planActive;
        CreatedAt = DateTime.UtcNow;
    }
    public int Id { get; }
    public string? Plan { get; private set; }
    public bool PlanActive { get; private set; }
    public int DayUploadCount { get; private set; }
    public long StoredSize { get; private set; }
    public IEnumerable<Member>? Members { get; private set; }
    public DateTime CreatedAt { get; }
    public IEnumerable<Transfer>? Transfers { get; private set; }

    public void AddMember(User user, bool owner = false)
    {
        var members = Members?.ToList() ?? [];
        members.Add(new Member(user.Id, Id, owner));
        Members = members;
    }
    public void AddTransfer(Transfer transfer)
    {
        var transfers = Transfers?.ToList() ?? [];
        transfers.Add(transfer);
        Transfers = transfers;
        IncrementStoredSize(transfer.Size);
        IncrementUploadCount(transfer.FilesCount);
    }
    private void IncrementUploadCount(int uploads) => DayUploadCount += uploads;
    
    private void IncrementStoredSize(long size) => StoredSize += size;
    
}
