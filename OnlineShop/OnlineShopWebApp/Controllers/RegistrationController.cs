using Microsoft.AspNetCore.Mvc;
using OnlineShopWebApp.Models;
using OnlineShop.Db.Interfaces;
using OnlineShopWebApp.Interfaces;
using OnlineShopWebApp.Data;

namespace OnlineShopWebApp.Controllers
{
    public class RegistrationController : Controller
    {
        private readonly IUserRepository _userRepository;

        public RegistrationController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(RegisterModel model)
        {
            if (model.Password == model.Login)
            {
                ModelState.AddModelError("Password", "Пароль не должен совпадать с логином.");
            }

            if (_userRepository.Exists(model.Login))
            {
                ModelState.AddModelError("Login", "Пользователь с таким логином уже существует.");
            }

            if (!ModelState.IsValid)
            {
                return View("Index", model);
            }

            _userRepository.Add(model.Login, model.Password);

            return RedirectToAction("Index", "Authorization");
        }
    }
}
