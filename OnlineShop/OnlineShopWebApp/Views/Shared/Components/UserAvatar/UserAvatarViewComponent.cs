using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Db.Models;
using OnlineShopWebApp.Helpers;

namespace OnlineShopWebApp.Views.Shared.Components.UserAvatar
{
    public class UserAvatarViewComponent : ViewComponent
    {
        private readonly UserManager<User> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserAvatarViewComponent(UserManager<User> userManager,
                                       IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public IViewComponentResult Invoke()
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext?.User?.Identity?.IsAuthenticated != true)
            {
                return Content("");
            }
                            
            var userId = UserIdHelper.GetUserId(httpContext);
            var user = _userManager.FindByNameAsync(userId).Result; 

            if (user == null)
            {
                return Content("");
            }
               
            var avatarPath = user.AvatarPath ?? "/img/default-avatar.jpg";
            var displayName = !string.IsNullOrWhiteSpace(user.FirstName)
                ? $"{user.FirstName} {user.LastName}".Trim()
                : user.UserName ?? "Пользователь";

            var model = new { AvatarPath = avatarPath, DisplayName = displayName };
            return View(model);
        }
    }
}
