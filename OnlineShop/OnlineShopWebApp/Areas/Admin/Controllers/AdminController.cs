using Microsoft.AspNetCore.Mvc;
using OnlineShopWebApp.Interfaces;
using OnlineShopWebApp.Models;

namespace OnlineShopWebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IRolesRepository _rolesRepository;

        public AdminController(IProductRepository productRepository, IOrderRepository orderRepository, IRolesRepository rolesRepository)
        {
            _productRepository = productRepository;
            _orderRepository = orderRepository;
            _rolesRepository = rolesRepository;
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
           var roles = _rolesRepository.GetAll();
           return View(roles);
        }

        public IActionResult AddRole()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddRole(Role role)
        {
            if(_rolesRepository.Exist(role.Name))
            {
                ModelState.AddModelError("Name", "Роль с таким наименованием уже существует!");
                return View(role);
            }

            if(!ModelState.IsValid)
            {
                return View(role);
            }

            _rolesRepository.Add(role);


            return RedirectToAction("Roles");
        }

        [HttpPost]
        public IActionResult DeleteRole(int roleId)
        {
            _rolesRepository.Delete(roleId);
            return RedirectToAction("Roles");
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
