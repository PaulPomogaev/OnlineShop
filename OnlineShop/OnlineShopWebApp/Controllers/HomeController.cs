using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Db.Interfaces;
using OnlineShopWebApp.Models;
using System.Text;
using OnlineShopWebApp.Helpers;
using OnlineShop.Core.Interfaces;

namespace OnlineShopWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly IReviewsApiService _reviewsApiService;
        private readonly ILogger<HomeController> _logger;

        public HomeController(IProductRepository productRepository, IReviewsApiService reviewsApiService, ILogger<HomeController> logger)
        {
            _productRepository = productRepository;
            _reviewsApiService = reviewsApiService;
            _logger = logger;
        }


        public async Task<IActionResult> Index() 
        {
            var products = _productRepository.GetAll();
            var productViewModels = products.ToViewModels();
                        
            foreach (var product in productViewModels)
            {
                try
                {
                    var rating = await _reviewsApiService.GetProductRatingAsync(product.Id);
                    product.Rating = rating.Rating;
                    product.ReviewCount = rating.ReviewCount;
                }
                catch (Exception ex)
                {
                     product.Rating = 0;
                    product.ReviewCount = 0;
                    _logger.LogWarning(ex, "Ќе удалось получить рейтинг дл€ продукта {ProductId}", product.Id);
                }
            }

            return View(productViewModels);
        }

        public IActionResult Search(string query)
        {
            var products = _productRepository.SearchEngine(query);
            return View(products);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
