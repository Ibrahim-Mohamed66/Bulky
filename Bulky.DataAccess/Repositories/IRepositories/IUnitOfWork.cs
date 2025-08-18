
namespace Bulky.DataAccess.Repositories.IRepositories;

public interface IUnitOfWork
{
    ICategoryRepository Category { get; }

    Task SaveChangesAsync();
}
