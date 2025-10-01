using Microsoft.AspNetCore.Mvc;
using OnlineShopWebApp.Models;

namespace OnlineShopWebApp.Controllers
{
    public class RegistrationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Register(RegisterModel model)
        {
            return RedirectToAction("Index", "Registration");
        }
    }
}
