using Microsoft.AspNetCore.Mvc;
using OnlineShop.Db.Models;
using OnlineShop.Db.Interfaces;
using OnlineShopWebApp.Models;
using OnlineShopWebApp.Helpers;
using Microsoft.AspNetCore.Authorization;
using System.Security.Principal;

namespace OnlineShopWebApp.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartRepository _cartRepository;

        public CartController(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        private string GetUserId()
        {
            if (User?.Identity == null || !User.Identity.IsAuthenticated)
            {
                return "guest";
            }

            if (string.IsNullOrEmpty(User.Identity.Name))
            {
                return "guest";
            }

            return User.Identity.Name;
        }

        public IActionResult Index()
        {
            var cart = _cartRepository.GetCart(GetUserId());
            return View(cart.ToViewModel());
        }

        public IActionResult Add(int productId, int quantity = 1)
        {
            _cartRepository.AddToCart(productId, quantity, GetUserId());
            return RedirectToAction("Index");
        }

        public IActionResult Remove(int itemId)
        {
            _cartRepository.RemoveFromCart(itemId, GetUserId());
            return RedirectToAction("Index");
        }

        public IActionResult Clear()
        {
            _cartRepository.ClearCart(GetUserId());
            return RedirectToAction("Index");
        }

        public IActionResult IncreaseQuantity(int itemId)
        {
            _cartRepository.IncreaseItemQuantity(itemId, GetUserId());

            return RedirectToAction("Index");
        }

        public IActionResult DecreaseQuantity(int itemId)
        {
            _cartRepository.DecreaseItemQuantity(itemId, GetUserId());
            
            return RedirectToAction("Index");
        }
    }
}
