using Microsoft.AspNetCore.Mvc;
using OnlineShopWebApp.Interfaces;
using OnlineShopWebApp.Models;


namespace OnlineShopWebApp.Controllers
{
    public class OrderController : Controller
    {
        private readonly ICartRepository _cartRepository;
        private readonly IOrderRepository _orderRepository;
       

        public OrderController(ICartRepository cartRepository, IOrderRepository orderRepository)
        {
            _cartRepository = cartRepository;
            _orderRepository = orderRepository;
        }

        public IActionResult Index()
        {
            var cart = _cartRepository.GetCart();

            if(cart.Items.Count == 0)
            {
                return RedirectToAction("Index", "Cart");
            }

            var order = _orderRepository.CreateOrder(cart);
            var viewModel = new OrderViewModel
            {
                Order = order,
                InputModel = new OrderInputModel()
            };
            return View(viewModel);
            
        }

        [HttpPost]
        public IActionResult Buy(OrderInputModel model)
        {
            var cart = _cartRepository.GetCart();
            if(cart.Items.Count == 0)
            {
                return RedirectToAction("Index", "Cart");
            }

            if(!ModelState.IsValid)
            {
                var orderForView = _orderRepository.CreateOrder(cart);
                var viewModel = new OrderViewModel
                {
                    Order = orderForView,
                    InputModel = model
                };
                return View("Index", viewModel);
            }

            var order = _orderRepository.CreateOrder(cart, model);

            _orderRepository.AddOrder(order);

            _cartRepository.ClearCart();

            return RedirectToAction("Success");
        }

        public IActionResult Success()
        {
            return View();
        }
    }
}
