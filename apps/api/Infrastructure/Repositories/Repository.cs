using System.Linq.Expressions;
using Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public abstract class Repository<TEntity, Key> : IRepository<TEntity, Key> where TEntity : class
{
    protected readonly DatabaseContext _context;
    protected readonly DbSet<TEntity> _dbSet;

    public Repository(DatabaseContext context)
    {
        _context = context;
        _dbSet = _context.Set<TEntity>();
    }
    public virtual TEntity Add(TEntity objModel) => _context.Set<TEntity>().Add(objModel).Entity;

    public virtual void Delete(TEntity objModel)
    {
        _dbSet.Attach(objModel);
        _dbSet.Remove(objModel);
    }
    public virtual void Delete(Key id)
    {
        TEntity? entityToDelete = _dbSet.Find(id);
        if (entityToDelete != null) Delete(entityToDelete);
    }

    public virtual void Update(TEntity objModel)
    {
        _dbSet.Attach(objModel);
        _context.Entry(objModel).State = EntityState.Modified;
    }
    public virtual async Task<TEntity?> GetByIdAsync(Key id, string? includeProperties = null, bool asNoTracking = true)
    {
        IQueryable<TEntity> query = _dbSet;
        if (includeProperties != null)
        {
            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }
        }
        if (asNoTracking) query = query.AsNoTracking();
        return await query.FirstOrDefaultAsync(entity => EF.Property<Key>(entity, "Id")!.Equals(id));
    }
    public virtual async Task<TEntity?> GetFirstAsync(Expression<Func<TEntity, bool>>? filter = null, string? includeProperties = null, bool asNoTracking = true)
    {
        IQueryable<TEntity> query = _dbSet;
        if (filter != null)
        {
            query = query.Where(filter);
        }
        if (includeProperties != null)
        {
            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }
        }
        return await query.AsNoTracking().FirstOrDefaultAsync();
    }

    public virtual async Task<List<TEntity>> GetListAsync(
        Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        string? includeProperties = null,
        int? limit = null,
        int? offset = null)
    {
        IQueryable<TEntity> query = _dbSet;
        if (filter != null)
        {
            query = query.Where(filter);
        }
        if (limit != null && offset != null) query = query.Skip(offset.Value).Take(limit.Value);
        if (includeProperties != null)
        {
            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }
        }
        if (orderBy != null)
        {
            return await orderBy(query).ToListAsync();
        }
        else
        {
            return await query.ToListAsync();
        }
    }
}