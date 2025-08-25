using Bulky.DataAccess.Repositories.IRepositories;
using Bulky.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BulkyWeb.Components
{
    public class CartViewComponent: ViewComponent
    {
        private readonly IUnitOfWork _unitOfWork;
        public CartViewComponent(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var claims = (ClaimsIdentity)User.Identity;
            var userId = claims.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId != null)
            {
                var cartCount = (await _unitOfWork.Cart.GetAllAsync(filter: c => c.ApplicationUserId == userId)).Count();
                HttpContext.Session.SetInt32(StaticData.SessionCart, cartCount);


                return View(HttpContext.Session.GetInt32(StaticData.SessionCart));
            }
            else
            {
                HttpContext.Session.Clear();
                return View(0);
            }
        }
    }
}
