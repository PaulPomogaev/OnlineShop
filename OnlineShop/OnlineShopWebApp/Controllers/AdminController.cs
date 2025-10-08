using Microsoft.AspNetCore.Mvc;
using OnlineShopWebApp.Interfaces;
using OnlineShopWebApp.Models;

namespace OnlineShopWebApp.Controllers
{
    public class AdminController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly IOrderRepository _orderRepository;

        public AdminController(IProductRepository productRepository, IOrderRepository orderRepository)
        {
            _productRepository = productRepository;
            _orderRepository = orderRepository;
        }

        public IActionResult Index()
        {
           return View();
        }

        public IActionResult Orders()
        {
            var orders = _orderRepository.GetAll();
            return View(orders);
        }

        public IActionResult DetailOrder(int orderId)
        {
            var order = _orderRepository.GetById(orderId);
            if(order == null)
            {
                return NotFound();
            }
            return View(order);
        }

        [HttpPost]
        public IActionResult UpdateOrder(Order order)
        {
            if (order == null || _orderRepository.GetById(order.Id) == null)
            {
                return NotFound();
            }

            _orderRepository.Update(order);
            return RedirectToAction("DetailOrder", new { orderId = order.Id });
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
