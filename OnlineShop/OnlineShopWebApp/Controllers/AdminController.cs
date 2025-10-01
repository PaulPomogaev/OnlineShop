using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace OnlineShopWebApp.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
           return View();
        }

        public IActionResult Orders()
        {
            return View("Orders");
        }

        public IActionResult Users()
        {
            return View("Users");
        }

        public IActionResult Roles()
        {
            return View("Roles");
        }

        public IActionResult Products()
        {
            return View("Products");
        }
    }
}
