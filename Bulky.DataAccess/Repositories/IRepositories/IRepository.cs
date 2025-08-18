using System.Linq.Expressions;

namespace Bulky.DataAccess.Repositories.IRepositories;

public interface IRepository<TEntity> where TEntity : class
{
    Task AddAsync(TEntity entity);
    Task<IEnumerable<TEntity>> GetAllAsync(int pageNumber = 1,
            int pageSize = 10, Expression<Func<TEntity, bool>>? filter = null
         , Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null);
    Task<TEntity?> GetByIdAsync(int id);
    Task<TEntity?> FindAsync(Expression<Func<TEntity, bool>> filter);
    Task RemoveAsync(int id);
    void RemoveRange(IEnumerable<TEntity> entities);
}
