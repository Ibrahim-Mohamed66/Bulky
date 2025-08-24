using Bulky.DataAccess.Repositories.IRepositories;
using Bulky.Models.Models;
using Bulky.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using System.Threading.Tasks;

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

        public async Task<IActionResult> Index(int page = 1, int pageSize = 10)
        {
            if(pageSize <= 0) pageSize = 10;
            if(page <= 1) page = 1; 
            var totalProducts = await _unitOfWork.Product.GetCountAsync(p => p.IsHidden == false);

            var totalPages = (int)Math.Ceiling((double)totalProducts / pageSize);
            page = Math.Max(1, Math.Min(page, totalPages));

            var products = await _unitOfWork.Product.GetAllAsync(
                filter: p => p.IsHidden == false,
                includes: p => p.Category,
                orderBy: q => q.OrderBy(p => p.DisplayOrder),
                pageNumber: page,
                pageSize: pageSize
            );

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalProducts = totalProducts;
            ViewBag.HasPreviousPage = page > 1;
            ViewBag.HasNextPage = page < totalPages;

            return View(products);
        }

        public async Task<IActionResult> Details(int productId)
        {
            var products = await _unitOfWork.Product.GetByIdAsync(productId, includes: p => p.Category);
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
                TempData["success"] = "Book added to cart successfully";
            }
            await _unitOfWork.SaveChangesAsync();
            return RedirectToAction("Index");
        }

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
                
                // Get updated cart count
                var cartItems = await _unitOfWork.Cart.GetAllAsync(filter: c => c.ApplicationUserId == userId);
                var cartCount = cartItems.Sum(c => c.Count);
                
                return Json(new { 
                    success = true, 
                    message = $"{product.Title} added to cart successfully!",
                    productTitle = product.Title,
                    cartCount = cartCount 
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "An error occurred while adding to cart" });
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetCartCount()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Json(new { count = 0 });
            }

            var claims = (ClaimsIdentity)User.Identity;
            var userId = claims.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var cartItems = await _unitOfWork.Cart.GetAllAsync(filter: c => c.ApplicationUserId == userId);
            var count = cartItems.Sum(c => c.Count);
            return Json(new { count = count });
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


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
