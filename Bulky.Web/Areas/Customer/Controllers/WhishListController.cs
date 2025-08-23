using Microsoft.AspNetCore.Mvc;

namespace BulkyWeb.Areas.Customer.Controllers
{
    public class WhishListController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
