using Microsoft.AspNetCore.Mvc;
using OnlineShop.Db.Models;
using OnlineShop.Db.Interfaces;
using OnlineShopWebApp.Models;
using OnlineShopWebApp.Helpers;
using Microsoft.AspNetCore.Authorization;


namespace OnlineShopWebApp.Controllers
{
    public class ComparisonController : BaseController
    {
        private readonly IComparisonRepository _comparisonRepository;
        private readonly IProductRepository _productRepository;

        public ComparisonController(IComparisonRepository comparisonRepository, IProductRepository productRepository)
        {
            _comparisonRepository = comparisonRepository;
            _productRepository = productRepository;
        }

        
        public IActionResult Index()
        {
            var comparison = _comparisonRepository.Get(GetUserId());
            if (comparison == null)
            {
                return View(new List<ProductViewModel>()); 
            }

            var products = comparison.Products.ToList();

            var productViewModels = products.ToViewModels();

            return View(productViewModels);
        }

        public IActionResult Add(int productId)
        {
            _comparisonRepository.Add(productId, GetUserId());
            return RedirectToAction("Index");
        }

        public IActionResult Remove(int productId)
        {
            _comparisonRepository.Remove(productId, GetUserId());
            return RedirectToAction("Index");
        }

        public IActionResult Clear()
        {
            _comparisonRepository.Clear(GetUserId());
            return RedirectToAction("Index");
        }
    }
}
