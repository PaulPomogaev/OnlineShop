using Microsoft.AspNetCore.Mvc;
using OnlineShopWebApp.Models;
using OnlineShop.Db.Interfaces;
using OnlineShopWebApp.Helpers;

namespace OnlineShopWebApp.Controllers
{
    public class FavoriteController : BaseController
    {
        private readonly IFavoriteRepository _favoriteRepository;
       
        
        public FavoriteController(IFavoriteRepository favoriteRepository)
        {
            _favoriteRepository = favoriteRepository;
            
        }
                
        public IActionResult Index()
        {
            var favorite = _favoriteRepository.Get(GetUserId());
            if (favorite == null)
            {
                return View(new List<ProductViewModel>());
            }

            var products = favorite.Products.ToList();

            var productViewModels = products.ToViewModels();

            return View(productViewModels);
        }

        public IActionResult Add(int productId)
        {
            _favoriteRepository.Add(productId, GetUserId());
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int productId)
        {
            _favoriteRepository.Remove(productId, GetUserId());
            return RedirectToAction("Index");
        }

        public IActionResult Clear()
        {
            _favoriteRepository.Clear(GetUserId());
            return RedirectToAction("Index");
        }
    }
}
