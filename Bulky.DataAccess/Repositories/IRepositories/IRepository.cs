using System.Linq.Expressions;

namespace Bulky.DataAccess.Repositories.IRepositories;

public interface IRepository<TEntity, TId> where TEntity : class
{
    Task AddAsync(TEntity entity);
    Task<IEnumerable<TEntity>> GetAllAsync(
                                            Expression<Func<TEntity, bool>>? filter = null,
                                            int pageNumber = 0,
                                            int pageSize = 0,
                                            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
                                            params Expression<Func<TEntity, object>>[]? includes);
    Task<TEntity?> GetByIdAsync(TId id  , params Expression<Func<TEntity, object>>[]? includes);
    Task<TEntity?> FilterAsync(Expression<Func<TEntity, bool>> filter, params Expression<Func<TEntity, object>>[]? includes);
    Task RemoveAsync(TId id);
    void RemoveRange(IEnumerable<TEntity> entities);
    Task<int> GetCountAsync(Expression<Func<TEntity, bool>>? filter = null);
    Task<bool> AnyAsync();
    Task<TEntity?> GetFirstOrDefaultAsync(Expression<Func<TEntity, bool>>? filter = null);


}
