using Azure;
using Bulky.DataAccess.Repositories.IRepositories;
using Bulky.Models.Models;
using Bulky.Models.ViewModels;
using Bulky.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using System.Security.Claims;
namespace BulkyWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index(string filter, int page = 1, int pageSize = 12)
        {
            if (pageSize <= 0) pageSize = 12;
            if (page <= 0) page = 1;

            Expression<Func<Product, bool>> predicate;
            if (!string.IsNullOrEmpty(filter) && filter.ToLower() != "all")
            {
                predicate = p => !p.IsHidden && p.Category.Name.ToLower() == filter.ToLower();
            }
            else
            {
                predicate = p => !p.IsHidden;
            }

            var totalProducts = await _unitOfWork.Product.GetCountAsync(predicate);

            // Calculate total pages
            var totalPages = (int)Math.Ceiling((double)totalProducts / pageSize);
            page = Math.Max(1, Math.Min(page, totalPages == 0 ? 1 : totalPages)); 

            // Get paginated filtered products
            var products = await _unitOfWork.Product.GetAllAsync(
                filter: predicate,
                pageNumber: page,
                pageSize: pageSize,
                orderBy: q => q.OrderBy(p => p.DisplayOrder),
                p => p.Category,
                p => p.ProductImages
            );

            // Pass pagination + filter info to view
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalProducts = totalProducts;
            ViewBag.HasPreviousPage = page > 1;
            ViewBag.HasNextPage = page < totalPages;
            ViewBag.SelectedFilter = filter ?? "all";

            return View(products);
        }


        public async Task<IActionResult> Details(int productId)
        {
            var claims = (ClaimsIdentity)User.Identity;
            var userId = claims.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId != null)
            {
                HttpContext.Session.SetInt32(StaticData.SessionCart,
                    (await _unitOfWork.Cart.GetAllAsync(filter: c => c.ApplicationUserId == userId)).Count());
            }
            var products = await _unitOfWork.Product.GetByIdAsync(productId, p => p.Category, p=>p.ProductImages);
            var cart = new Cart
            {
                Product = products,
                ProductId = products.Id,
                Count = 1
            };
            if (products == null || products.IsHidden)
            {
                return NotFound();
            }
            return View(cart);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Details(Cart ShopingCart)
        {
            var claims = (ClaimsIdentity)User.Identity;
            var userId = claims.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            ShopingCart.ApplicationUserId = userId;

            var existingCart = await _unitOfWork.Cart.FilterAsync(
                c => c.ApplicationUserId == ShopingCart.ApplicationUserId && c.ProductId == ShopingCart.ProductId);
            if (existingCart != null)
            {
                existingCart.Count += ShopingCart.Count;
                _unitOfWork.Cart.Update(existingCart);
                TempData["success"] = "Book count updated in cart successfully";
            }
            else
            {
                await _unitOfWork.Cart.AddAsync(ShopingCart);
                HttpContext.Session.SetInt32(StaticData.SessionCart,
                    (await _unitOfWork.Cart.GetAllAsync(filter: c => c.ApplicationUserId == userId)).Count());
                TempData["success"] = "Book added to cart successfully";
            }
            await _unitOfWork.SaveChangesAsync();
            return RedirectToAction("Details", new { productId = ShopingCart.ProductId });
        }

        
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> OrderHistory()
        {
            var claims = (ClaimsIdentity)User.Identity;
            var userId = claims.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var orders = await _unitOfWork.OrderHeader.GetAllAsync(
                filter: o => o.ApplicationUserId == userId,
                orderBy: q => q.OrderByDescending(o => o.OrderDate));

            var orderIds = orders.Select(o => o.Id).ToList();
            var orderDetails = await _unitOfWork.OrderDetail.GetAllAsync(
                filter: od => orderIds.Contains(od.OrderHeaderId),
                includes: od => od.Product);
            var orderVm = new OrdersHistoryVM()
            {
                OrderHeaderList = orders ,
                OrderDetailList = orderDetails
            };

            return View(orderVm);
        }

        #region API_Calls
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> AddToCart(int productId, int count)
        {
            try
            {
                var claims = (ClaimsIdentity)User.Identity;
                var userId = claims.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                // Get product to validate and for response
                var product = await _unitOfWork.Product.GetByIdAsync(productId);
                if (product == null)
                {
                    return Json(new { success = false, message = "Product not found" });
                }

                var existingCart = await _unitOfWork.Cart.FilterAsync(
                    c => c.ApplicationUserId == userId && c.ProductId == productId);

                if (existingCart != null)
                {
                    existingCart.Count += count;
                    _unitOfWork.Cart.Update(existingCart);
                }
                else
                {
                    var newCartItem = new Cart
                    {
                        ProductId = productId,
                        ApplicationUserId = userId,
                        Count = count
                    };
                    await _unitOfWork.Cart.AddAsync(newCartItem);
                }

                await _unitOfWork.SaveChangesAsync();
                HttpContext.Session.SetInt32(StaticData.SessionCart,
                    (await _unitOfWork.Cart.GetAllAsync(filter: c => c.ApplicationUserId == userId)).Count());

                var cartItems = await _unitOfWork.Cart.GetAllAsync(filter: c => c.ApplicationUserId == userId);
                var cartCount = cartItems.Count();

                HttpContext.Session.SetInt32(StaticData.SessionCart, cartCount);

                return Json(new
                {
                    success = true,
                    message = $"{product.Title} added to cart successfully!",
                    productTitle = product.Title,
                    count = cartCount
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "An error occurred while adding to cart" });
            }
        }
    }
    #endregion
}
