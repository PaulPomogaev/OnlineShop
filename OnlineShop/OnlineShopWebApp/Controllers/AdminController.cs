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
            ViewData["Title"] = "Панель администратора";
            return View();
        }

        public IActionResult Orders()
        {
            ViewData["Title"] = "Заказы";
            return View("Orders");
        }

        public IActionResult Users()
        {
            ViewData["Title"] = "Пользователи";
            return View("Users");
        }

        public IActionResult Roles()
        {
            ViewData["Title"] = "Роли";
            return View("Roles");
        }

        public IActionResult Products()
        {
            var products = _productRepository.GetAll();
            ViewData["Title"] = "Товары";
            return View("Products", products);
        }

        public IActionResult ProductDetails(int id)
        {
            var product = _productRepository.GetById(id);

            if (product == null)
            {
                return NotFound();
            }
            ViewData["Title"] = "Посмотреть подробно";
            return View("ProductDetails", product);
        }

        public IActionResult EditProduct(int id)
        {
            var product = _productRepository.GetById(id);

            if (product == null)
            {
                return NotFound();
            }
            ViewData["Title"] = "Редактирование товара";
            return View("EditProduct", product);
        }

        [HttpPost]
        public IActionResult EditProduct(Product product)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Title"] = "Редактирование товара";
                return View("EditProduct", product);
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
            ViewData["Title"] = "Добавление нового продукта";
            return RedirectToAction("CreateProduct");
        }

        [HttpPost]
        public IActionResult CreateProduct(Product product)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Title"] = "Добавление нового продукта";
                return View("CreateProduct", product);
            }

            product.PhotoPath = "img/whey-protein.jpg";
            _productRepository.Add(product);
            return RedirectToAction("Products");
        }
    }
}
