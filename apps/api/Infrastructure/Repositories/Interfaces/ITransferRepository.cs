using Domain;

namespace Infrastructure.Repositories.Interfaces;

public interface ITransferRepository : IRepository<Transfer, int> {
    Task<IEnumerable<Transfer>> GetExpiredTransfers();
    void SetAsExpired(Transfer transfer);
    Task<Transfer?> GetByKeyWithFiles(string key, int limit, int offset);
}
