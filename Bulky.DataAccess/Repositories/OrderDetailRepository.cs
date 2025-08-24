using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repositories.IRepositories;
using Bulky.Models.Models;


namespace Bulky.DataAccess.Repositories;

public class OrderDetailRepository : Repository<OrderDetail, int>, IOrderDetailRepository
{
    private readonly BulkyDbContext _context;

    public OrderDetailRepository(BulkyDbContext context) : base(context)
    {
        _context = context;
    }

    public void Update(OrderDetail orderDetail)
    {
        _context.OrderDetails.Update(orderDetail);
    }
}
