
using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repositories.IRepositories;

namespace Bulky.DataAccess.Repositories;

public class UnitOfWork: IUnitOfWork
{
    private readonly BulkyDbContext _dbContext;

    public UnitOfWork(BulkyDbContext dbContext)
    {
        _dbContext = dbContext;
        Category = new CategoryRepository(_dbContext);
    }

    public ICategoryRepository Category { get; private set; }


    public async Task SaveChangesAsync()
    {
        await _dbContext.SaveChangesAsync();
    }
}
