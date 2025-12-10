using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Db.Interfaces;
using OnlineShopWebApp.Models;
using System.Text;
using OnlineShopWebApp.Helpers;
using OnlineShop.Core.Interfaces;
using OnlineShop.Db.Models;

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
            try
            {
               var productIds = productViewModels.Select(p => p.Id).ToList();
               var ratings = await _reviewsApiService.GetProductRatingsAsync(productIds);
               foreach(var product in productViewModels)
                {
                    var ratingDto = ratings.FirstOrDefault(r => r.ProductId == product.Id);
                    if(ratingDto!=null)
                    {
                        product.Rating = ratingDto.Rating;
                        product.ReviewCount = ratingDto.ReviewCount;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Ќе удалось получить рейтинги дл€ списка продуктов");
                foreach(var product in productViewModels)
                {
                    product.Rating = 0;
                    product.ReviewCount = 0;
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
