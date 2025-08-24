using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repositories.IRepositories;
using Bulky.Models.Models;


namespace Bulky.DataAccess.Repositories;

public class OrderHeadeRepository : Repository<OrderHeader, int>, IOrderHeaderRepository
{
    private readonly BulkyDbContext _context;

    public OrderHeadeRepository(BulkyDbContext context) : base(context)
    {
        _context = context;
    }

    public void Update(OrderHeader orderHeader)
    {
        _context.OrderHeaders.Update(orderHeader);
    }

    public void UpdateStatus(int id, string orderStatus, string? paymentStatus = null)
    {
        var orderHeader = _context.OrderHeaders.FirstOrDefault(o => o.Id == id);
        if (orderHeader != null)
        {
            orderHeader.OrderStatus = orderStatus;
            if (paymentStatus != null)
            {
                orderHeader.PaymentStatus = paymentStatus;
            }
        }
    }

    public void UpdateStripePaymentId(int id, string sessionId, string paymentIntentId)
    {
        var orderHeader = _context.OrderHeaders.FirstOrDefault(o => o.Id == id);
        if (orderHeader != null)
        {
            if(!String.IsNullOrEmpty(sessionId))
            {
                orderHeader.SessionId = sessionId;
            }
            if (!String.IsNullOrEmpty(paymentIntentId))
            {
                orderHeader.PaymentIntentId = paymentIntentId;
                orderHeader.PaymentDate = DateTime.Now;
            }
        }
    }
}
