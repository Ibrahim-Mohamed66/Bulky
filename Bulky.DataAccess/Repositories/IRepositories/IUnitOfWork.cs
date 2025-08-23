
namespace Bulky.DataAccess.Repositories.IRepositories;

public interface IUnitOfWork
{
    ICategoryRepository Category { get; }
    IProductRepository Product { get; }
    ICompanyRepository Company { get; }
    ICartRepository Cart { get; }
    IApplicationUserRepository ApplicationUser { get; }

    Task SaveChangesAsync();
}
