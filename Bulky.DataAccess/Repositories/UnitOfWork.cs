
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
        Product = new ProductRepository(_dbContext);
        Company = new CompanyRepository(_dbContext);
        Cart = new CartRepository(_dbContext);
        ApplicationUser = new ApplicationUserRepository(_dbContext);
        OrderHeader = new OrderHeadeRepository(_dbContext);
        OrderDetail = new OrderDetailRepository(_dbContext);
        ProductImage = new ProductImageRepository(_dbContext);
        Section = new SectionRepository(_dbContext);
        Banner = new BannerRepository(_dbContext);
        Store = new StoreRepository(_dbContext);
        ProductStore = new ProductStoreRepository(_dbContext);
    }

    public ICategoryRepository Category { get; private set; }
    public IProductRepository Product { get; private set; }
    public ICompanyRepository Company { get; private set; }
    public ICartRepository Cart { get; private set; }
    public IApplicationUserRepository ApplicationUser { get; private set; }
    public IOrderHeaderRepository OrderHeader { get; private set; }
    public IOrderDetailRepository OrderDetail { get; private set; }
    public IProductImageRepository ProductImage { get; private set; }
    public ISectionRepository Section { get; private set; }
    public IBannerRepository Banner { get; private set; }
    public IStoreRepository Store { get; private set; }
    public IProductStoreRepository ProductStore { get; private set; }


    public async Task SaveChangesAsync()
    {
        await _dbContext.SaveChangesAsync();
    }
}
