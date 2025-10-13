using Microsoft.AspNetCore.Mvc;
using OnlineShopWebApp.Models;
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

            var user = new User
            {
                Login = model.Login,
                PasswordHash = UserJsonRepository.HashPassword(model.Password)
            };

            _userRepository.Add(user);

            return RedirectToAction("Index", "Authorization");
        }
    }
}
