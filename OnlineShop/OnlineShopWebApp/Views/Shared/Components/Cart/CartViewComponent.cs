using Microsoft.AspNetCore.Mvc;
using OnlineShop.Db.Interfaces;

namespace OnlineShopWebApp.Views.Shared.Components.Cart
{
    public class CartViewComponent : ViewComponent
    {
        private readonly ICartRepository _cartRepository;

        public CartViewComponent(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        public IViewComponentResult Invoke()
        {
            var userId = "guest";
            if(HttpContext.User?.Identity?.IsAuthenticated == true)
            {
                userId = HttpContext.User.Identity.Name;
            }
            var itemCount = _cartRepository.GetCartItemCount(userId);
            return View("Cart", itemCount);
        }
    }
}