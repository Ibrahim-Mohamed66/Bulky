using Bulky.DataAccess.Repositories.IRepositories;
using Bulky.Models.Models;
using Bulky.Models.ViewModels;
using Bulky.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Stripe.Checkout;
using Stripe.Climate;

namespace BulkyWeb.Areas.Admin.Controllers;
[Area("Admin")]
[Authorize(Roles = StaticData.Role_Admin + "," + StaticData.Role_Employee)]
public class OrderController : Controller
{
    private readonly IUnitOfWork _unitOfWork;
    [BindProperty]
    public OrderVM OrderVM { get; set; }
    public OrderController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task<IActionResult> Index()
    {
        var orders = await _unitOfWork.OrderHeader.GetAllAsync(orderBy: o => o.OrderByDescending(oh => oh.OrderDate), includes: oh => oh.ApplicationUser);
        return View(orders);
    }

    public async Task<IActionResult> GetAll(string status)
    {
        if (!string.IsNullOrEmpty(status) && status.ToLower() != "all")
        {
            return Json(new { data = await _unitOfWork.OrderHeader.GetAllAsync(filter: o => o.OrderStatus.ToLower() == status.ToLower(), includes: o => o.ApplicationUser, orderBy: o => o.OrderByDescending(oh => oh.OrderDate)) });
        }
        return Json(new { data = await _unitOfWork.OrderHeader.GetAllAsync(includes: o => o.ApplicationUser) });
    }
    public async Task<IActionResult> Details(int orderId)
    {
        var orderHeader = await _unitOfWork.OrderHeader.GetByIdAsync(orderId, includes: o => o.ApplicationUser);
        if (orderHeader == null)
        {
            return NotFound();
        }
        var orderDetails = await _unitOfWork.OrderDetail.GetAllAsync(filter: o => o.OrderHeader.Id == orderId, includes: o => o.Product);
        OrderVM = new()
        {
            OrderHeader = orderHeader,
            OrderDetail = orderDetails
        };
        return View(OrderVM);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateOrderDetail()
    {
        if (OrderVM.OrderHeader.Id == 0)
        {
            return NotFound();
        }
        var orderHeaderFromDb = await _unitOfWork.OrderHeader.GetByIdAsync(OrderVM.OrderHeader.Id);
        if (orderHeaderFromDb == null)
        {
            return NotFound();
        }
        orderHeaderFromDb.FirstName = OrderVM.OrderHeader.FirstName;
        orderHeaderFromDb.LastName = OrderVM.OrderHeader.LastName;
        orderHeaderFromDb.PhoneNumber = OrderVM.OrderHeader.PhoneNumber;
        orderHeaderFromDb.StreetAddress = OrderVM.OrderHeader.StreetAddress;
        orderHeaderFromDb.City = OrderVM.OrderHeader.City;
        orderHeaderFromDb.State = OrderVM.OrderHeader.State;
        orderHeaderFromDb.PostalCode = OrderVM.OrderHeader.PostalCode;
        if (!string.IsNullOrEmpty(OrderVM.OrderHeader.Carrier))
        {
            orderHeaderFromDb.Carrier = OrderVM.OrderHeader.Carrier;
        }
        if (!string.IsNullOrEmpty(OrderVM.OrderHeader.TrackingNumber))
        {
            orderHeaderFromDb.TrackingNumber = OrderVM.OrderHeader.TrackingNumber;
        }
        _unitOfWork.OrderHeader.Update(orderHeaderFromDb);
        await _unitOfWork.SaveChangesAsync();
        TempData["success"] = "Order Details Updated Successfully";
        return RedirectToAction(nameof(Details), new { orderId = orderHeaderFromDb.Id });
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> StartProcessing()
    {
        if (OrderVM.OrderHeader.Id == 0)
        {
            return NotFound();
        }
        var orderHeaderFromDb = await _unitOfWork.OrderHeader.GetByIdAsync(OrderVM.OrderHeader.Id);
        if (orderHeaderFromDb == null)
        {
            return NotFound();
        }
        _unitOfWork.OrderHeader.UpdateStatus(orderHeaderFromDb.Id, StaticData.StatusInProcess);

        await _unitOfWork.SaveChangesAsync();
        TempData["success"] = "Order Status Updated Successfully";
        return RedirectToAction(nameof(Details), new { orderId = orderHeaderFromDb.Id });
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ShipOrder()
    {
        if (OrderVM.OrderHeader.Id == 0)
        {
            return NotFound();
        }
        var orderHeaderFromDb = await _unitOfWork.OrderHeader.GetByIdAsync(OrderVM.OrderHeader.Id);
        if (orderHeaderFromDb == null)
        {
            return NotFound();
        }
        if (string.IsNullOrEmpty(OrderVM.OrderHeader.Carrier) || string.IsNullOrEmpty(OrderVM.OrderHeader.TrackingNumber))
        {
            TempData["error"] = "Carrier and Tracking Number are required to ship the order.";
            return RedirectToAction(nameof(Details), new { orderId = orderHeaderFromDb.Id });
        }
        orderHeaderFromDb.Carrier = OrderVM.OrderHeader.Carrier;
        orderHeaderFromDb.TrackingNumber = OrderVM.OrderHeader.TrackingNumber;
        orderHeaderFromDb.OrderStatus = StaticData.StatusShipped;
        orderHeaderFromDb.ShippingDate = DateTime.Now;
        if (orderHeaderFromDb.PaymentStatus == StaticData.PaymentStatusDelayedPayment)
        {
            orderHeaderFromDb.PaymentDueDate = DateOnly.FromDateTime(DateTime.Now.AddDays(30));
        }
        _unitOfWork.OrderHeader.Update(orderHeaderFromDb);
        await _unitOfWork.SaveChangesAsync();
        TempData["success"] = "Order Shipped Successfully";
        return RedirectToAction(nameof(Details), new { orderId = orderHeaderFromDb.Id });
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CancelOrder()
    {
        if (OrderVM.OrderHeader.Id == 0)
        {
            return NotFound();
        }
        var orderHeaderFromDb = await _unitOfWork.OrderHeader.GetByIdAsync(OrderVM.OrderHeader.Id);
        if (orderHeaderFromDb == null)
        {
            return NotFound();
        }
        if (orderHeaderFromDb.PaymentStatus == StaticData.PaymentStatusApproved)
        {
            var options = new RefundCreateOptions
            {
                Reason = RefundReasons.RequestedByCustomer,
                PaymentIntent = orderHeaderFromDb.PaymentIntentId
            };
            var service = new RefundService();
            Refund refund = service.Create(options);
            _unitOfWork.OrderHeader.UpdateStatus(orderHeaderFromDb.Id, StaticData.StatusCancelled, StaticData.StatusRefunded);
        }
        else
        {
            _unitOfWork.OrderHeader.UpdateStatus(orderHeaderFromDb.Id, StaticData.StatusCancelled, StaticData.StatusCancelled);
        }
        await _unitOfWork.SaveChangesAsync();
        TempData["success"] = "Order Cancelled Successfully";
        return RedirectToAction(nameof(Details), new { orderId = orderHeaderFromDb.Id });

    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DetailsPayNow()
    {
        if (OrderVM.OrderHeader.Id == 0)
        {
            return NotFound();
        }
        OrderVM.OrderHeader = await _unitOfWork.OrderHeader.GetByIdAsync(OrderVM.OrderHeader.Id);
        OrderVM.OrderDetail = await _unitOfWork.OrderDetail.GetAllAsync(filter: o => o.OrderHeader.Id == OrderVM.OrderHeader.Id, includes: o => o.Product);
        if (OrderVM.OrderHeader == null)
        {
            return NotFound();
        }
        var domain = "https://localhost:7072";
        var options = new SessionCreateOptions
        {
            SuccessUrl = $"{domain}/admin/order/PaymentConfirmation?OrderHeaderId={OrderVM.OrderHeader.Id}",
            CancelUrl = $"{domain}/admin/order/index",
            LineItems = new List<SessionLineItemOptions>(),

            Mode = "payment",
        };
        foreach (var item in OrderVM.OrderDetail)
        {
            var sessionItem = new SessionLineItemOptions
            {
                PriceData = new SessionLineItemPriceDataOptions
                {
                    UnitAmount = (long)(item.Product.Price * 100), // Amount in cents
                    Currency = "usd",
                    ProductData = new SessionLineItemPriceDataProductDataOptions
                    {
                        Name = item.Product.Title
                    }
                },
                Quantity = item.Count
            };
            options.LineItems.Add(sessionItem);
        }
        var service = new SessionService();
        Session session = service.Create(options);
        _unitOfWork.OrderHeader.UpdateStripePaymentId(OrderVM.OrderHeader.Id, session.Id, session.PaymentIntentId);
        await _unitOfWork.SaveChangesAsync();
        Response.Headers.Add("Location", session.Url);
        return new StatusCodeResult(303);
    }

    [HttpGet]
    public async Task<IActionResult> PaymentConfirmation(int orderHeaderId)
    {
        var orderHeader = await _unitOfWork.OrderHeader.GetByIdAsync(orderHeaderId,
            includes: o => o.ApplicationUser);

        if (orderHeader == null)
            return NotFound();


        if (orderHeader.PaymentStatus == StaticData.PaymentStatusDelayedPayment)
        {
            var service = new SessionService();
            Session session = service.Get(orderHeader.SessionId);
            if (session.PaymentStatus.ToLower() == "paid")
            {
                _unitOfWork.OrderHeader.UpdateStripePaymentId(orderHeaderId, session.Id, session.PaymentIntentId);
                _unitOfWork.OrderHeader.UpdateStatus(orderHeaderId, orderHeader.OrderStatus, StaticData.PaymentStatusApproved);
                await _unitOfWork.SaveChangesAsync();
                orderHeader = await _unitOfWork.OrderHeader.GetByIdAsync(orderHeaderId,
                    includes: o => o.ApplicationUser);
            }
        }

        // Get order details to display in confirmation
        var orderDetails = await _unitOfWork.OrderDetail.GetAllAsync(
            filter: od => od.OrderHeaderId == orderHeaderId,
            includes: od => od.Product);

        OrderVM = new()
        {
            OrderHeader = orderHeader,
            OrderDetail = orderDetails
        };
        return View(OrderVM);

    }
}
