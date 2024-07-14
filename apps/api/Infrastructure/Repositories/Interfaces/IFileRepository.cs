namespace Infrastructure.Repositories.Interfaces;

public interface IFileRepository : IRepository<Domain.File, long> {
    Task<IEnumerable<Domain.File>> GetByTransferAsync(int transferId, int limit, int offset, string? includeProperties = null, bool asNoTracking = true);
    Task<int> CountByTransferAsync(int transferId);
    Task UpdateRangeAsync(IEnumerable<Domain.File> files);
    Task<IEnumerable<Domain.File>> GetByIds(IEnumerable<long> ids);
}
