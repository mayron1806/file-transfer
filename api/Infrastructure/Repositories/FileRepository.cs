using Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class FileRepository(DatabaseContext context) : Repository<Domain.File, long>(context), IFileRepository
    {
        public Task<int> CountByTransferAsync(int transferId)
        {
            return _dbSet.CountAsync(x => x.TransferId == transferId);
        }
        public async Task<IEnumerable<Domain.File>> GetByIds(IEnumerable<long> ids)
        {
            var query = _dbSet.Where(x => ids.ToList().Contains(x.Id));
            return await query.ToListAsync();
        }
        public async Task<IEnumerable<Domain.File>> GetByTransferAsync(int transferId, int limit, int offset, string? includeProperties = null, bool asNoTracking = true)
        {
            var query = _dbSet.Where(x => x.TransferId == transferId);
            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties.Split
                    (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }
            if (asNoTracking) query = query.AsNoTracking();
            return await query.Skip(offset).Take(limit).ToListAsync();
        }
        public async Task UpdateRangeAsync(IEnumerable<Domain.File> files) {
            _context.Files.UpdateRange(files);
            await _context.SaveChangesAsync();
        }
    }
}