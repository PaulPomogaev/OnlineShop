using Microsoft.AspNetCore.Mvc;

namespace OnlineShopWebApp.Views.Shared.Components.AdminMenu
{
    public class AdminMenuViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View("AdminMenu");
        }
    }
}
