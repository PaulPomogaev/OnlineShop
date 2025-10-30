using Microsoft.AspNetCore.Mvc;
using OnlineShop.Db.Interfaces;
using OnlineShop.Core.Models;
using OnlineShopWebApp.Interfaces;
using OnlineShop.Db.Models;
using OnlineShopWebApp.Models;

namespace OnlineShopWebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OrdersController : Controller
    {
        private readonly IOrderRepository _orderRepository;

        public OrdersController(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public IActionResult Index()
        {
            var orders = _orderRepository.GetAll();
            var orderViewModels = orders.Select(o => new OrderViewModel
            {
                Id = o.Id,
                UserId = o.UserId,
                CreatedDate = o.CreatedDate,
                DeliveryDate = o.DeliveryDate,
                Comment = o.Comment,
                Status = o.Status,
                Customer = o.Customer,
                Items = o.Items.Select(item => new OrderItemViewModel
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
            }).ToList();

            return View(orderViewModels);
        }

        public IActionResult Detail(int orderId)
        {
            var order = _orderRepository.GetById(orderId);
            if (order == null)
            {
                return NotFound();
            }
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
        public IActionResult Edit(OrderViewModel orderViewModel)
        {
            if (orderViewModel == null)
            {
                return NotFound();
            }

            var order = _orderRepository.GetById(orderViewModel.Id);
            if (order == null)
            {
                return NotFound();
            }

            order.Status = orderViewModel.Status;

            _orderRepository.Edit(order);

            return RedirectToAction("Detail", new { orderId = order.Id });
        }
    }
}
