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
            return View(order);
        }

        [HttpPost]
        public IActionResult Buy(OrderInputModel model)
        {
            var cart = _cartRepository.GetCart();
            if(cart.Items.Count == 0)
            {
                return RedirectToAction("Index", "Cart");
            }

            var oder = _orderRepository.CreateOrder(cart, model);

            _orderRepository.AddOrder(oder);

            _cartRepository.ClearCart();

            return RedirectToAction("Success");
        }

        public IActionResult Success()
        {
            return View();
        }
    }
}
