using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using OnlineShopWebApp.Models;
using OnlineShopWebApp.Helpers;
using OnlineShop.Core.Interfaces;
using OnlineShop.Core.Interfaces.Cqrs;
using OnlineShop.Core.Models.Products.Queries;

namespace OnlineShopWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IReviewsApiService _reviewsApiService;
        private readonly ILogger<HomeController> _logger;

        public HomeController(IMediator mediator, IReviewsApiService reviewsApiService, ILogger<HomeController> logger)
        {
            _mediator = mediator;
            _reviewsApiService = reviewsApiService;
            _logger = logger;
        }

        public async Task<IActionResult> Index() 
        {
            var products = await _mediator.Send(new GetAllProductsQuery());
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
                    else
                    {
                        product.Rating = 0;
                        product.ReviewCount = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Не удалось получить рейтинги для списка продуктов");
                foreach(var product in productViewModels)
                {
                    product.Rating = 0;
                    product.ReviewCount = 0;
                }    
            }
           
            return View(productViewModels);
        }

        public async Task<IActionResult> Search(string query)
        {
            var products = await _mediator.Send(new SearchProductsQuery(query));
            var productViewModels = products.Select(p => p.ToViewModel()).ToList();
            try
            {
                var productIds = productViewModels.Select(p => p.Id).ToList();
                var ratings = await _reviewsApiService.GetProductRatingsAsync(productIds);

                foreach (var product in productViewModels)
                {
                    var ratingDto = ratings.FirstOrDefault(r => r.ProductId == product.Id);
                    if (ratingDto != null)
                    {
                        product.Rating = ratingDto.Rating;
                        product.ReviewCount = ratingDto.ReviewCount;
                    }
                    else
                    {
                        product.Rating = 0;
                        product.ReviewCount = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Не удалось получить рейтинги для найденных продуктов");
                foreach (var product in productViewModels)
                {
                    product.Rating = 0;
                    product.ReviewCount = 0;
                }
            }
            return View(productViewModels);
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
