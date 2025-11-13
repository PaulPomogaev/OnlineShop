using Microsoft.AspNetCore.Mvc;
using OnlineShop.Db.Interfaces;
using OnlineShopWebApp.Helpers;

namespace OnlineShopWebApp.Views.Shared.Components.Cart
{
    public class CartViewComponent : ViewComponent
    {
        private readonly ICartRepository _cartRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CartViewComponent(ICartRepository cartRepository, IHttpContextAccessor httpContextAccessor)
        {
            _cartRepository = cartRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public IViewComponentResult Invoke()
        {
            var httpContext = _httpContextAccessor.HttpContext;
            var userId = UserIdHelper.GetUserId(httpContext);
            var itemCount = _cartRepository.GetCartItemCount(userId);
            return View("Cart", itemCount);
        }
    }
}