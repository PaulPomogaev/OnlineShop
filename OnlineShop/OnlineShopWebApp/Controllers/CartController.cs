using Microsoft.AspNetCore.Mvc;
using OnlineShop.Db.Models;
using OnlineShop.Db.Interfaces;
using OnlineShopWebApp.Models;

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
            var cart = _cartRepository.GetCart();
            var cartViewModel = new CartViewModel
            {
                Id = cart.Id,
                UserId = cart.UserId,
                Items = cart.Items.Select(item => new CartItemViewModel
                {
                    Id = item.Id,
                    Quantity = item.Quantity,
                    Product = new ProductViewModel
                    {
                        Id = item.Product.Id,
                        Name = item.Product.Name,
                        Cost = item.Product.Cost,
                        Description = item.Product.Description
                    }
                }).ToList()
            };

            return View(cartViewModel);
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
