
using Bulky.Models.Models;

namespace Bulky.DataAccess.Repositories.IRepositories;

public interface ICartRepository : IRepository<Cart, int>
{
    void Update(Cart cart);
}
