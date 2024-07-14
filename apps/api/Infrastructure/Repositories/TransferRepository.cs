using Domain;
using Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class TransferRepository(DatabaseContext context) : Repository<Transfer, int>(context), ITransferRepository
{
    public async Task<IEnumerable<Transfer>> GetExpiredTransfers() 
    {
        var query = _dbSet.Where(x => x.ExpiresAt < DateTime.UtcNow && !x.Expired);
        return await query.AsNoTracking().ToListAsync();
    }
    public void SetAsExpired(Transfer transfer) {
        transfer.SetAsExpired();
        Update(transfer);
    }
    public async Task<Transfer?> GetByKeyWithFiles(string key, int limit, int offset) {
        var transfer = await _dbSet
            .Where(x => x.Key == key)
            .Include(x => x.Files.Take(limit).Skip(offset))
            .AsNoTracking()
            .FirstOrDefaultAsync();
        return transfer;
    }
}