using Microsoft.AspNetCore.Mvc;
using OnlineShopWebApp.Interfaces;
using OnlineShopWebApp.Models;

namespace OnlineShopWebApp.Controllers
{
    public class ComparisonController : Controller
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
            var comparison = _comparisonRepository.Get();
            if(comparison == null)
            {
                return View(new List<Product>());
            }
            var products = comparison.ProductIds.Select(id => _productRepository.GetById(id)).Where(p => p != null).ToList();
            return View(products);
        }

        public IActionResult Add(int productId)
        {
            _comparisonRepository.Add(productId);
            return RedirectToAction("Index");
        }

        public IActionResult Remove(int productId)
        {
            _comparisonRepository.Remove(productId);
            return RedirectToAction("Index");
        }

        public IActionResult Clear()
        {
            _comparisonRepository.Clear();
            return RedirectToAction("Index");
        }
    }
}
