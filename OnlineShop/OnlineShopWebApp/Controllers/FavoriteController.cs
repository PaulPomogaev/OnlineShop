using Microsoft.AspNetCore.Mvc;
using OnlineShopWebApp.Interfaces;

namespace OnlineShopWebApp.Controllers
{
    public class FavoriteController : Controller
    {
        private readonly IFavoriteRepository _favoriteRepository;
        private readonly IProductRepository _productRepository;

        public FavoriteController(IFavoriteRepository favoriteRepository, IProductRepository productRepository)
        {
            _favoriteRepository = favoriteRepository;
            _productRepository = productRepository;
        }

        public IActionResult Index()
        {
            var favorite = _favoriteRepository.Get();
            var products = favorite.ProductIds.Select(id => _productRepository.GetById(id)).Where(p => p != null).ToList();
            return View(products);
        }

        [HttpPost]
        public IActionResult Add(int productId)
        {
            _favoriteRepository.Add(productId);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Delete(int productId)
        {
            _favoriteRepository.Remove(productId);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Clear()
        {
            _favoriteRepository.Clear();
            return RedirectToAction("Index");
        }
    }
}
