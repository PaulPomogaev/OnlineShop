using Microsoft.AspNetCore.Mvc;
using OnlineShopWebApp.Data;
using OnlineShopWebApp.Models;
using System.Text;

namespace OnlineShopWebApp.Controllers

{
    public class ProductController : Controller
    {
        private readonly ProductJsonRepository _productRepository;

        public ProductController(ProductJsonRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public IActionResult Index(int id)
        {
            var product = _productRepository.ReturnById(id);
            
            return View(product);
        }
    }
}
