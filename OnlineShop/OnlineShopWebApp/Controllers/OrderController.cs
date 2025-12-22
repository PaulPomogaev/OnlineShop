using Microsoft.AspNetCore.Mvc;
using OnlineShopWebApp.Models;
using OnlineShop.Db.Interfaces;
using OnlineShopWebApp.Helpers;
using Microsoft.AspNetCore.Authorization;


namespace OnlineShopWebApp.Controllers
{
    [Authorize]
    public class OrderController : BaseController
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
            try
            {
                var userId = GetUserId();
                var cart = _cartRepository.GetCart(userId);

                if (cart == null || cart.Items.Count == 0)
                {
                    return RedirectToAction("Index", "Cart");
                }

                var order = _orderRepository.Create(cart);
                var viewModel = order.ToViewModel();
                                
                return View(viewModel);
            }
            catch (Exception ex)
            {
                return Content($"Error: {ex.Message}");
            }
        }

        [HttpPost]
        public IActionResult Buy(OrderViewModel orderViewModel)
        {
            var userId = GetUserId();
            var cart = _cartRepository.GetCart(userId);
            if (cart.Items.Count == 0)
            {
                return RedirectToAction("Index", "Cart");
            }

            if (!ModelState.IsValid)
            {
                var orderForView = _orderRepository.Create(cart, orderViewModel.InputModel);

                orderViewModel = orderForView.ToViewModel();

                orderViewModel.InputModel = orderViewModel.InputModel;

                return View("Index", orderViewModel);
            }

            var order = _orderRepository.Create(cart, orderViewModel.InputModel);
                        
            _orderRepository.SaveOrder(order);

            _cartRepository.ClearCart(userId);

            return RedirectToAction("Success");
        }

        public IActionResult Success()
        {
            return View();
        }
                
    }
}
