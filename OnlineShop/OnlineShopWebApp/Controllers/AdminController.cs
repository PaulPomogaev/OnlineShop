using Microsoft.AspNetCore.Mvc;
using OnlineShopWebApp.Interfaces;
using OnlineShopWebApp.Models;
using System.Reflection;

namespace OnlineShopWebApp.Controllers
{
    public class AdminController : Controller
    {
        private readonly IProductRepository _productRepository;

        public AdminController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public IActionResult Index()
        {
           return View();
        }

        public IActionResult Orders()
        {
            return View();
        }

        public IActionResult Users()
        {
            return View();
        }

        public IActionResult Roles()
        {
           return View();
        }

        public IActionResult Products()
        {
            var products = _productRepository.GetAll();
            return View(products);
        }

        public IActionResult ProductDetails(int id)
        {
            var product = _productRepository.GetById(id);

            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        public IActionResult EditProduct(int id)
        {
            var product = _productRepository.GetById(id);

            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpPost]
        public IActionResult EditProduct(Product product)
        {
            if (!ModelState.IsValid)
            {
                return View(product);
            }
            _productRepository.Update(product);
            return RedirectToAction("Products");
        }

        public IActionResult DeleteProduct(int id)
        {
            _productRepository.Delete(id);
            return RedirectToAction("Products");
        }

        public IActionResult CreateProduct()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateProduct(Product product)
        {
            if (!ModelState.IsValid)
            {
                return View(product);
            }

            product.PhotoPath = "img/whey-protein.jpg";
            _productRepository.Add(product);
            return RedirectToAction("Products");
        }
    }
}
