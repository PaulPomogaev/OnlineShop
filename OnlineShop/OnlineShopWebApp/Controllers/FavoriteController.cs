using Microsoft.AspNetCore.Mvc;
using OnlineShopWebApp.Interfaces;
using OnlineShopWebApp.Models;

namespace OnlineShopWebApp.Controllers
{
    public class FavoriteController : Controller
    {
        private readonly IFavoriteRepository _favoriteRepository;
        private readonly IProductRepository _productRepository;
        private readonly ICartRepository _cartRepository;


        public FavoriteController(IFavoriteRepository favoriteRepository, IProductRepository productRepository, ICartRepository cartRepository)
        {
            _favoriteRepository = favoriteRepository;
            _productRepository = productRepository;
            _cartRepository = cartRepository;
        }

        public IActionResult Index()
        {
            ViewBag.CartItemCount = _cartRepository.GetCartItemCount();
            var favorite = _favoriteRepository.Get();
            if(favorite == null)
            {
                return View(new List<Product>());
            }

            var products = favorite.ProductIds.Select(id => _productRepository.GetById(id)).Where(p => p != null).ToList();
            return View(products);
        }

        public IActionResult Add(int productId)
        {
            _favoriteRepository.Add(productId);
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int productId)
        {
            _favoriteRepository.Remove(productId);
            return RedirectToAction("Index");
        }

        public IActionResult Clear()
        {
            _favoriteRepository.Clear();
            return RedirectToAction("Index");
        }
    }
}
