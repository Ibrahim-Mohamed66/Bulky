using Bulky.DataAccess.Repositories.IRepositories;
using Bulky.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;

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
                includes: c => c.Product);

            var cartVM = new CartVM
            {
                CartItems = cartItems,
                OrderTotal = cartItems.Sum(c => c.TotalPrice)
            };
            return View(cartVM);
        }

        public async Task<IActionResult> Plus(int cartId)
        {
            var cartItem = await _unitOfWork.Cart.GetByIdAsync(cartId, c => c.Product);
            if (cartItem == null)
                return NotFound();

            cartItem.Count++;
            _unitOfWork.Cart.Update(cartItem);
            await _unitOfWork.SaveChangesAsync();
            return RedirectToAction("Index");

        }

        public async Task<IActionResult> Minus(int cartId)
        {
            var cartItem = await _unitOfWork.Cart.GetByIdAsync(cartId, c => c.Product);
            if (cartItem == null)
                return RedirectToAction("Index");

            if (cartItem.Count <= 1)
            {
                await _unitOfWork.Cart.RemoveAsync(cartId);
                await _unitOfWork.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            cartItem.Count--;
            _unitOfWork.Cart.Update(cartItem);
            await _unitOfWork.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Remove(int cartId)
        {
            var cartItem = await _unitOfWork.Cart.GetByIdAsync(cartId, c => c.Product);
            if (cartItem == null)
                return Json(new { success = false, message = "Cart item not found" });

            var userId = cartItem.ApplicationUserId;
            await _unitOfWork.Cart.RemoveAsync(cartId);
            await _unitOfWork.SaveChangesAsync();

            return RedirectToAction("Index");
        }


    }
}
