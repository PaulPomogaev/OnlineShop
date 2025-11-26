using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Db.Models;
using OnlineShopWebApp.Models;

namespace OnlineShopWebApp.Controllers
{
    [Authorize]
    public class ChangePasswordController : Controller
    {
        private readonly UserManager<User> _userManager;

        public ChangePasswordController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(ChangePassword model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = _userManager.GetUserAsync(User).Result;
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var result = _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword).Result;

            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = "Пароль успешно изменен!";
                return RedirectToAction("Index", "UserProfile", new { area = "UserProfile" });
            }

            foreach (var error in result.Errors)
            {
                if (error.Code.Contains("PasswordMismatch"))
                {
                    ModelState.AddModelError("OldPassword", "Неверный старый пароль");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }
    }
}
