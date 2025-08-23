using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repositories.IRepositories;
using Bulky.Models.Models;


namespace Bulky.DataAccess.Repositories;

public class CartRepository : Repository<Cart, int>, ICartRepository
{
    private readonly BulkyDbContext _context;

    public CartRepository(BulkyDbContext context) : base(context)
    {
        _context = context;
    }

    public void Update(Cart cart)
    {
        _context.Carts.Update(cart);
    }

}
