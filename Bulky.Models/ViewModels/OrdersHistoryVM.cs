using Bulky.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.Models.ViewModels;

public class OrdersHistoryVM
{
    public IEnumerable<OrderHeader> OrderHeaderList { get; set; }
    public IEnumerable<OrderDetail> OrderDetailList { get; set; }
}
