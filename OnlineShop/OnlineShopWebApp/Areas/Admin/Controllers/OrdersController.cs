using Microsoft.AspNetCore.Mvc;
using OnlineShop.Db.Interfaces;
using OnlineShop.Core.Models;
using OnlineShopWebApp.Interfaces;
using OnlineShop.Db.Models;
using OnlineShopWebApp.Models;
using OnlineShopWebApp.Helpers;
using Microsoft.AspNetCore.Authorization;
using OnlineShop.Db;

namespace OnlineShopWebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = Constants.AdminRoleName)]
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
            return View(orders.ToViewModels());
        }

        public IActionResult Detail(int orderId)
        {
            var order = _orderRepository.GetById(orderId);
            if (order == null)
            {
                return NotFound();
            }
            return View(order.ToViewModel());
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
