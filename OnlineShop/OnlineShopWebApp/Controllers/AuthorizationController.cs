using Microsoft.AspNetCore.Mvc;
using OnlineShopWebApp.Data;
using OnlineShopWebApp.Interfaces;
using OnlineShopWebApp.Models;
using OnlineShop.Db.Repostories;
using OnlineShop.Db.Interfaces;

namespace OnlineShopWebApp.Controllers
{
    public class AuthorizationController : Controller
    {
        private readonly IUserRepository _userRepository;

        public AuthorizationController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginModel model)
        {
            var user = _userRepository.GetByLogin(model.Login);
            if(user == null || !UserDbRepository.VerifyPassword(model.Password, user.PasswordHash))
            {
                ModelState.AddModelError("", "Неверный логин или пароль");
            }

            if (ModelState.IsValid)
            {
                return RedirectToAction("Index");
            }
            return View("Index", model);
        }

    }
}
