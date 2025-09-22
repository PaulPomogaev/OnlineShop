using Microsoft.AspNetCore.Mvc;
using OnlineShopWebApp.Data;

namespace OnlineShopWebApp.Controllers
{
    public class CartController : Controller
    {
        private readonly CartJsonRepository _cartRepository;

        public CartController(CartJsonRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        public IActionResult Index()
        {
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
    }
}
