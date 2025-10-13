using Microsoft.AspNetCore.Mvc;

namespace OnlineShopWebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UsersController : Controller
    {
        public IActionResult Users()
        {
            return View();
        }
    }
}
