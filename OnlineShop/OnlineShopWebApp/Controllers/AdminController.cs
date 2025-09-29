using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace OnlineShopWebApp.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            ViewData["Title"] = "Панель администратора";
            return View();
        }

        public IActionResult Orders()
        {
            ViewData["Title"] = "Заказы";
            return View("Orders");
        }

        public IActionResult Users()
        {
            ViewData["Title"] = "Пользователи";
            return View("Users");
        }

        public IActionResult Roles()
        {
            ViewData["Title"] = "Роли";
            return View("Roles");
        }

        public IActionResult Products()
        {
            ViewData["Title"] = "Товары";
            return View("Products");
        }
    }
}
