using Microsoft.AspNetCore.Mvc;
using OnlineShopWebApp.Interfaces;

namespace OnlineShopWebApp.Controllers
{
    public class ContactsController : Controller
    {
        private readonly ICartRepository _cartRepository;

        public ContactsController(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        public IActionResult Index()
        {
            ViewBag.CartItemCount = _cartRepository.GetCartItemCount();
            return View();
        }
    }
}
