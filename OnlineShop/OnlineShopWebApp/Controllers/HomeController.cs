using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using OnlineShopWebApp.Data;
using OnlineShopWebApp.Models;
using OnlineShopWebApp.Data;
using System.Text;

namespace OnlineShopWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();

        }

        [Route("Home/Index")]
        public IActionResult HomeIndex()
        {
            var products = ProductJsonRepository.GetAll();
            var stringBuilder = new StringBuilder();
            foreach (var product in products)
            {
                stringBuilder.AppendLine($"Id{product.Id}");
                stringBuilder.AppendLine($"Name{product.Name}");
                stringBuilder.AppendLine($"Cost{product.Cost}");
                stringBuilder.AppendLine();
            }

            return Content(stringBuilder.ToString(), "text/plain");
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
