using Microsoft.AspNetCore.Mvc;
using OnlineShopWebApp.Data;

namespace OnlineShopWebApp.Controllers
{
    public class CartController : Controller
    {
        public IActionResult Index()
        {
            var cart = CartJsonRepository.GetCart();
            return View(cart);
        }

        public IActionResult Add(int productId, int quantity = 1)
        {
            CartJsonRepository.AddToCart(productId, quantity);
            return RedirectToAction("Index");
        }

        public IActionResult Remove(int itemId)
        {
            CartJsonRepository.RemoveFromCart(itemId);
            return RedirectToAction("Index");
        }

        public IActionResult Clear()
        {
            CartJsonRepository.ClearCart();
            return RedirectToAction("Index");
        }
    }
}
