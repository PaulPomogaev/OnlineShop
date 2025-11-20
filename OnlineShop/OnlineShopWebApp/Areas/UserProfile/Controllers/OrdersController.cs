using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineShopWebApp.Controllers;
using OnlineShopWebApp.Helpers;
using OnlineShop.Db.Interfaces;

namespace OnlineShopWebApp.Areas.UserProfile.Controllers
{
    [Area("UserProfile")]
    [Authorize]
    public class OrdersController : BaseController
    {
        private readonly IOrderRepository _orderRepository;
       
        public OrdersController(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public IActionResult Index()
        {
            var userId = GetUserId();
            var orders = _orderRepository.GetUserOrders(userId);
            var viewModel = orders.Select(o => o.ToViewModel()).ToList();
            return View(viewModel);
        }

        public IActionResult Detail(int id)
        {
            var userId = GetUserId();
            var order = _orderRepository.GetById(id);

            if(order == null || order.UserId != userId)
            {
                return Forbid();
            }
            
            return View(order.ToViewModel());
        }
    }
}
