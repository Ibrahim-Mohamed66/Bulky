using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Bulky.DataAccess.Repositories;

public class Repository<TEntity, TId> : IRepository<TEntity, TId> where TEntity : class
{
    private readonly BulkyDbContext _context;
    private readonly DbSet<TEntity> _dbSet;

    public Repository(BulkyDbContext context)
    {
        _context = context;
        _dbSet = context.Set<TEntity>();
    }

    public async Task AddAsync(TEntity entity)
    {
        await _dbSet.AddAsync(entity);
    }

    public async Task<TEntity?> FilterAsync(
    Expression<Func<TEntity, bool>> filter,
    params Expression<Func<TEntity, object>>[] includes)
    {
        IQueryable<TEntity> query = _dbSet.AsNoTracking();

        // Apply includes
        foreach (var include in includes)
        {
            query = query.Include(include);
        }

        // Apply filter
        query = query.Where(filter);

        return await query.FirstOrDefaultAsync();
    }


    public async Task<IEnumerable<TEntity>> GetAllAsync(
        int pageNumber = 1,
        int pageSize = 10,
        Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        params Expression<Func<TEntity, object>>[]? includes)
    {
        // Ensure pageNumber and pageSize are valid
        if (pageNumber < 1) pageNumber = 1;
        if (pageSize < 1) pageSize = 10;


        IQueryable<TEntity> query = _dbSet.AsNoTracking();

        if (filter != null)
            query = query.Where(filter);
        if (includes != null && includes.Length > 0)
        {
            foreach (var include in includes)
                query = query.Include(include);
        }

        if (orderBy != null)
            query = orderBy(query);

        return await query.Skip((pageNumber - 1) * pageSize)
                          .Take(pageSize)
                          .ToListAsync();
    }

    public async Task<TEntity?> GetByIdAsync(TId id, params Expression<Func<TEntity, object>>[]? includes)
    {
        IQueryable<TEntity> query = _dbSet.AsNoTracking();
        if (includes != null)
        {
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
        }
        return await query.FirstOrDefaultAsync(e => EF.Property<TId>(e, "Id")!.Equals(id));
    }

    public async Task<int> GetCountAsync(Expression<Func<TEntity, bool>>? filter = null)
    {
        IQueryable<TEntity> query = _dbSet.AsNoTracking();
        if (filter != null)
        {
            query = query.Where(filter);
        }
        return await query.CountAsync();
    }

    public async Task RemoveAsync(TId id)
    {
        var entity = await _dbSet.FindAsync(id);
        if (entity != null)
        {
            _dbSet.Remove(entity);
        }
    }

    public void RemoveRange(IEnumerable<TEntity> entities)
    {
        _dbSet.RemoveRange(entities);
    }
}
