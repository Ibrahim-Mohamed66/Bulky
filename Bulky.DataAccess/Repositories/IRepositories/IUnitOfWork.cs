
namespace Bulky.DataAccess.Repositories.IRepositories;

public interface IUnitOfWork
{
    ICategoryRepository Category { get; }
    IProductRepository Product { get; }
    ICompanyRepository Company { get; }
    ICartRepository Cart { get; }
    IApplicationUserRepository ApplicationUser { get; }
    IOrderHeaderRepository OrderHeader { get; }
    IOrderDetailRepository OrderDetail { get; }
    IProductImageRepository ProductImage { get; }  
    ISectionRepository Section { get; }

    Task SaveChangesAsync();
}
