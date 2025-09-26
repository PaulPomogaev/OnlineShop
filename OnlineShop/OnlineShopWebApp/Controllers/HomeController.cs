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
        private readonly ProductJsonRepository _productRepository;

        public HomeController(ProductJsonRepository productRepository)
        {
            _productRepository = productRepository;
        }

               
        public IActionResult Index()
        {
            var products = _productRepository.GetAll();
            
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
