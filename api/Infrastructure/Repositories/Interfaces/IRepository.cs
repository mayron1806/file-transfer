using System.Linq.Expressions;

namespace Infrastructure.Repositories.Interfaces;

public interface IRepository<T, K> where T : class
{
    T Add(T objModel);
    void Update(T objModel);
    void Delete(K id);
    void Delete(T objModel);
    Task<T?> GetByIdAsync(K id, string? includeProperties = null, bool asNoTracking = true);
    Task<T?> GetFirstAsync(Expression<Func<T, bool>>? filter = null, string? includeProperties = null, bool asNoTracking = true);
    Task<List<T>> GetListAsync(
        Expression<Func<T, bool>>? filter = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        string? includeProperties = null,
        int? limit = null,
        int? offset = null);
}