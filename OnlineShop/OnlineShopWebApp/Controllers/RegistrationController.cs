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

        [HttpPost]
        public IActionResult Register(RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", model);
            }

            if (model.Password == model.Login)
            {
                ModelState.AddModelError("Password", "Пароль не должен совпадать с логином.");
                return View("Index", model);
            }

            return RedirectToAction("Index", "Authorization");
        }
    }
}
