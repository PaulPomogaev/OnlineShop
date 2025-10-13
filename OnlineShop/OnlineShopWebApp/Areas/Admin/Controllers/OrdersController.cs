using Microsoft.AspNetCore.Mvc;
using OnlineShopWebApp.Interfaces;
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

        public IActionResult Orders()
        {
            var orders = _orderRepository.GetAll();
            return View(orders);
        }

        public IActionResult DetailOrder(int orderId)
        {
            var order = _orderRepository.GetById(orderId);
            if (order == null)
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
    }
}
