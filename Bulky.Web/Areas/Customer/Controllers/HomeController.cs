using System.Diagnostics;
using Bulky.DataAccess.Repositories.IRepositories;
using Bulky.Models.Models;
using Microsoft.AspNetCore.Mvc;

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


        public async Task<IActionResult> Details(int id)
        {
            var products = await _unitOfWork.Product.GetByIdAsync(id, includes: p => p.Category);
            if (products == null || products.IsHidden)
            {
                return NotFound();
            }
            return View(products);
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
