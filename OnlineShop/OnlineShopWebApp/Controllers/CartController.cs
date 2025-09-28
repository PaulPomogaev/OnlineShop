using Microsoft.AspNetCore.Mvc;
using OnlineShopWebApp.Data;
using OnlineShopWebApp.Interfaces;

namespace OnlineShopWebApp.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartRepository _cartRepository;

        public CartController(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        public IActionResult Index()
        {
            ViewBag.CartItemCount = _cartRepository.GetCartItemCount();
            var cart = _cartRepository.GetCart();
            return View(cart);
        }

        public IActionResult Add(int productId, int quantity = 1)
        {
            _cartRepository.AddToCart(productId, quantity);
            return RedirectToAction("Index");
        }

        public IActionResult Remove(int itemId)
        {
            _cartRepository.RemoveFromCart(itemId);
            return RedirectToAction("Index");
        }

        public IActionResult Clear()
        {
            _cartRepository.ClearCart();
            return RedirectToAction("Index");
        }

        public IActionResult IncreaseQuantity(int itemId)
        {
            _cartRepository.IncreaseItemQuantity(itemId);

            return RedirectToAction("Index");
        }

        public IActionResult DecreaseQuantity(int itemId)
        {
            _cartRepository.DecreaseItemQuantity(itemId);
            
            return RedirectToAction("Index");
        }
    }
}
