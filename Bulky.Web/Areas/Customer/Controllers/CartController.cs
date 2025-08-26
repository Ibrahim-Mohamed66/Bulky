using Bulky.DataAccess.Repositories.IRepositories;
using Bulky.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Bulky.Models.Models;
using Bulky.Utility;
using Stripe.Checkout;

namespace BulkyWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class CartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CartController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var id = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            var cartItems = await _unitOfWork.Cart.GetAllAsync(
                filter: c => c.ApplicationUserId == id,
                pageNumber: 0,
                pageSize: 0,
                orderBy: c => c.OrderByDescending(c => c.Id),
                c => c.Product,
                c => c.Product.ProductImages);

            var cartVM = new CartVM
            {
                CartItems = cartItems,
                OrderHeader = new OrderHeader
                {
                    OrderTotal = cartItems.Sum(c => c.TotalPrice)
                }
            };
            return View(cartVM);
        }
        [HttpGet]
        public async Task<IActionResult> Summary()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var id = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            var cartItems = await _unitOfWork.Cart.GetAllAsync(
                filter: c => c.ApplicationUserId == id,
                0,0,null,
                 c => c.Product,
                 c=>c.Product.ProductImages);

            var cartVM = new CartVM
            {
                CartItems = cartItems,
                OrderHeader = new OrderHeader
                {
                    OrderTotal = cartItems.Sum(c => c.TotalPrice)
                }
            };

            cartVM.OrderHeader.ApplicationUser = await _unitOfWork.ApplicationUser.GetByIdAsync(id);
            if (cartVM.OrderHeader.ApplicationUser != null)
            {
                // Prefill shipping info from user profile
                cartVM.OrderHeader.FirstName = cartVM.OrderHeader.ApplicationUser.FirstName;
                cartVM.OrderHeader.LastName = cartVM.OrderHeader.ApplicationUser.LastName;
                cartVM.OrderHeader.PhoneNumber = cartVM.OrderHeader.ApplicationUser.PhoneNumber;
                cartVM.OrderHeader.StreetAddress = cartVM.OrderHeader.ApplicationUser.StreetAddress;
                cartVM.OrderHeader.City = cartVM.OrderHeader.ApplicationUser.City;
                cartVM.OrderHeader.State = cartVM.OrderHeader.ApplicationUser.State;
                cartVM.OrderHeader.PostalCode = cartVM.OrderHeader.ApplicationUser.PostalCode;
            }

            return View(cartVM);
        }

        [HttpPost]
        [ActionName("Summary")]
        public async Task<IActionResult> SummaryPost(CartVM cartVM)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var id = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            var cartItems = await _unitOfWork.Cart.GetAllAsync(
                filter: c => c.ApplicationUserId == id,
                includes: c => c.Product);

            cartVM.CartItems = cartItems;
            cartVM.OrderHeader.ApplicationUserId = id;
            cartVM.OrderHeader.OrderDate = DateTime.Now;
            cartVM.OrderHeader.OrderTotal = cartItems.Sum(c => c.TotalPrice);

            var user = await _unitOfWork.ApplicationUser.GetByIdAsync(id);

            // Determine status based on company association
            if (user.CompanyId.GetValueOrDefault() == 0)
            {
                cartVM.OrderHeader.OrderStatus = StaticData.StatusPending;
                cartVM.OrderHeader.PaymentStatus = StaticData.PaymentStatusPending;
            }
            else
            {
                cartVM.OrderHeader.OrderStatus = StaticData.StatusApproved;
                cartVM.OrderHeader.PaymentStatus = StaticData.PaymentStatusDelayedPayment;
            }

            // Save order header
            await _unitOfWork.OrderHeader.AddAsync(cartVM.OrderHeader);
            await _unitOfWork.SaveChangesAsync();

            // Save order details
            foreach (var item in cartItems)
            {
                var orderDetail = new OrderDetail
                {
                    ProductId = item.ProductId,
                    OrderHeaderId = cartVM.OrderHeader.Id,
                    Price = item.TotalPrice,
                    Count = item.Count
                };
                await _unitOfWork.OrderDetail.AddAsync(orderDetail);

            }
            await _unitOfWork.SaveChangesAsync();
            if (user.CompanyId.GetValueOrDefault() == 0)
            {
                var domain = "https://localhost:7072";
                var options = new SessionCreateOptions
                {
                    SuccessUrl = $"{domain}/customer/cart/OrderConfirmation?id={cartVM.OrderHeader.Id}",
                    CancelUrl = $"{domain}/customer/cart/index",
                    LineItems = new List<SessionLineItemOptions>(),

                    Mode = "payment",
                };
                foreach (var item in cartItems)
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
                _unitOfWork.OrderHeader.UpdateStripePaymentId(cartVM.OrderHeader.Id, session.Id, session.PaymentIntentId);
                await _unitOfWork.SaveChangesAsync();
                Response.Headers.Add("Location", session.Url);
                return new StatusCodeResult(303);
            }
            return RedirectToAction("OrderConfirmation", new { id = cartVM.OrderHeader.Id });
        }

        [HttpGet]
        public async Task<IActionResult> OrderConfirmation(int id)
        {
            var orderHeader = await _unitOfWork.OrderHeader.GetByIdAsync(id,
                includes: o => o.ApplicationUser);

            if (orderHeader == null)
                return NotFound();


            if(orderHeader.PaymentStatus != StaticData.PaymentStatusDelayedPayment)
            {
                var service = new SessionService();
                Session session = service.Get(orderHeader.SessionId);
                if(session.PaymentStatus.ToLower() == "paid")
                {
                    _unitOfWork.OrderHeader.UpdateStripePaymentId(id, session.Id, session.PaymentIntentId);
                    _unitOfWork.OrderHeader.UpdateStatus(id, StaticData.StatusApproved, StaticData.PaymentStatusApproved);
                    await _unitOfWork.SaveChangesAsync();
                    orderHeader = await _unitOfWork.OrderHeader.GetByIdAsync(id,
                        includes: o => o.ApplicationUser);
                }
            }

            // Get order details to display in confirmation
            var orderDetails = await _unitOfWork.OrderDetail.GetAllAsync(
                filter: od => od.OrderHeaderId == id,
                0,
                0,
                orderBy: od => od.OrderByDescending(od => od.Id),
                 od => od.Product,
                 od => od.Product.ProductImages);

            // Clear cart items after successful order
            var cartItems = await _unitOfWork.Cart.GetAllAsync(
                filter: c => c.ApplicationUserId == orderHeader.ApplicationUserId);
            _unitOfWork.Cart.RemoveRange(cartItems);
            await _unitOfWork.SaveChangesAsync();

            // Create view model with order details
            var cartVM = new CartVM
            {
                OrderHeader = orderHeader,
                CartItems = orderDetails.Select(od => new Cart
                {
                    Id = od.Id,
                    ProductId = od.ProductId,
                    Product = od.Product,
                    Count = od.Count,
                    ApplicationUserId = orderHeader.ApplicationUserId,

                }).ToList()
            };

            return View(cartVM);
        }


        [HttpPost]
        public async Task<IActionResult> Plus(int cartId)
        {
            var cartItem = await _unitOfWork.Cart.GetByIdAsync(cartId, c => c.Product);
            if (cartItem == null)
                return NotFound();

            cartItem.Count++;
            _unitOfWork.Cart.Update(cartItem);
            await _unitOfWork.SaveChangesAsync();
            HttpContext.Session.SetInt32(StaticData.SessionCart,
                (await _unitOfWork.Cart.GetAllAsync(filter: c => c.ApplicationUserId == cartItem.ApplicationUserId)).Count());
            TempData["success"] = "Cart updated successfully";
            return RedirectToAction("Index");

        }
        [HttpPost]
        public async Task<IActionResult> Minus(int cartId)
        {
            var cartItem = await _unitOfWork.Cart.GetByIdAsync(cartId, c => c.Product,c=>c.Product.ProductImages);
            if (cartItem == null)
                return RedirectToAction("Index");

            if (cartItem.Count <= 1)
            {
                await _unitOfWork.Cart.RemoveAsync(cartId);
                await _unitOfWork.SaveChangesAsync();
                HttpContext.Session.SetInt32(StaticData.SessionCart,
                    (await _unitOfWork.Cart.GetAllAsync(filter: c => c.ApplicationUserId == cartItem.ApplicationUserId)).Count());

                TempData["success"] = "Item removed from cart";

                return RedirectToAction("Index");
            }

            cartItem.Count--;
            _unitOfWork.Cart.Update(cartItem);
            await _unitOfWork.SaveChangesAsync();
            TempData["success"] = "Cart updated successfully";

            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> Remove(int cartId)
        {
            var cartItem = await _unitOfWork.Cart.GetByIdAsync(cartId, c => c.Product, c => c.Product.ProductImages);
            if (cartItem == null)
                return NotFound();

            var userId = cartItem.ApplicationUserId;
            await _unitOfWork.Cart.RemoveAsync(cartId);
            await _unitOfWork.SaveChangesAsync();
            HttpContext.Session.SetInt32(StaticData.SessionCart,
                (await _unitOfWork.Cart.GetAllAsync(filter: c => c.ApplicationUserId == userId)).Count());
            TempData["success"] = "Item removed from cart";

            return RedirectToAction("Index");
        }

    }
}
