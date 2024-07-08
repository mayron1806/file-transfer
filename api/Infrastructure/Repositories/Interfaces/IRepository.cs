using System.Linq.Expressions;

namespace Infrastructure.Repositories.Interfaces;

public interface IRepository<T, K> where T : class
{
    T Add(T objModel);
    void Update(T objModel);
    void Delete(K id);
    void Delete(T objModel);
    Task<T?> GetByIdAsync(K id);
    Task<T?> GetFirstAsync(Expression<Func<T, bool>>? filter = null, string? includeProperties = null, bool asNoTracking = true);
    Task<List<T>> GetListAsync(
        Expression<Func<T, bool>>? filter = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        Func<IQueryable<T>, IQueryable<T>>? include = null,
        string? includeProperties = null);
}