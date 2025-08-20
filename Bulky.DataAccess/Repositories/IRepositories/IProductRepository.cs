
using Bulky.Models.Models;

namespace Bulky.DataAccess.Repositories.IRepositories;

public interface IProductRepository:IRepository<Product,int>
{
    void Update(Product product);
    Task<(IEnumerable<Product> Products, int TotalCount)> SearchAsync(string keyword, int pageNumber = 1, int pageSize = 10);
}
