using Microsoft.AspNetCore.Mvc;
using OnlineShopWebApp.Models;
using OnlineShop.Db.Interfaces;
using OnlineShop.Core.Models;
using OnlineShopWebApp.Helpers;


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

            if (cart.Items.Count == 0)
            {
                return RedirectToAction("Index", "Cart");
            }

            var order = _orderRepository.Create(cart);

            var viewModel = order.ToViewModel();
            
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Buy(OrderViewModel orderViewModel)
        {
            var cart = _cartRepository.GetCart();
            if (cart.Items.Count == 0)
            {
                return RedirectToAction("Index", "Cart");
            }

            if (!ModelState.IsValid)
            {
                var orderForView = _orderRepository.Create(cart);

                orderViewModel = orderForView.ToViewModel();

                orderViewModel.InputModel = orderViewModel.InputModel;

                return View("Index", orderViewModel);
            }

            var order = _orderRepository.Create(cart, orderViewModel.InputModel);

            _orderRepository.Add(order);

            _cartRepository.ClearCart();

            return RedirectToAction("Success");
        }

        public IActionResult Success()
        {
            return View();
        }
    }
}
