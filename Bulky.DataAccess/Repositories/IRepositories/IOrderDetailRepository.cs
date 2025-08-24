
using Bulky.Models.Models;

namespace Bulky.DataAccess.Repositories.IRepositories;

public interface IOrderDetailRepository: IRepository<OrderDetail,int>
{
    void Update(OrderDetail orderDetail);
}
