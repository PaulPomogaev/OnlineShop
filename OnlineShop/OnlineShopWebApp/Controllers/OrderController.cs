using Microsoft.AspNetCore.Mvc;
using OnlineShopWebApp.Models;
using OnlineShop.Db.Interfaces;
using OnlineShop.Core.Models;


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

            var viewModel = new OrderViewModel
            {
                Id = order.Id,
                UserId = order.UserId,
                CreatedDate = order.CreatedDate,
                DeliveryDate = order.DeliveryDate,
                Comment = order.Comment,
                Status = order.Status,
                Customer = order.Customer,
                Items = order.Items.Select(item => new OrderItemViewModel
                {
                    Id = item.Id,
                    Quantity = item.Quantity,
                    Product = new ProductViewModel
                    {
                        Id = item.Product.Id,
                        Name = item.Product.Name,
                        Cost = item.Product.Cost,
                        Description = item.Product.Description
                    }
                }).ToList(),
                InputModel = new OrderInputModel()
            };

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

                orderViewModel = new OrderViewModel
                {
                    Id = orderForView.Id,
                    UserId = orderForView.UserId,
                    CreatedDate = orderForView.CreatedDate,
                    DeliveryDate = orderForView.DeliveryDate,
                    Comment = orderForView.Comment,
                    Status = orderForView.Status,
                    Customer = orderForView.Customer,
                    Items = orderForView.Items.Select(item => new OrderItemViewModel
                    {
                        Id = item.Id,
                        Quantity = item.Quantity,
                        Product = new ProductViewModel
                        {
                            Id = item.Product.Id,
                            Name = item.Product.Name,
                            Cost = item.Product.Cost,
                            Description = item.Product.Description
                        }
                    }).ToList(),
                    InputModel = orderViewModel.InputModel
                };

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
