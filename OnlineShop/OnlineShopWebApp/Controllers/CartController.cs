using Microsoft.AspNetCore.Mvc;
using OnlineShop.Db.Interfaces;
using OnlineShopWebApp.Helpers;

namespace OnlineShopWebApp.Controllers
{
    public class CartController : BaseController
    {
        private readonly ICartRepository _cartRepository;

        public CartController(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
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
