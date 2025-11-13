using Microsoft.AspNetCore.Mvc;
using OnlineShopWebApp.Models;
using OnlineShop.Db.Models;
using OnlineShop.Db.Interfaces;
using Microsoft.AspNetCore.Authorization;
using OnlineShop.Db;
using Microsoft.AspNetCore.Identity;

namespace OnlineShopWebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = Constants.AdminRoleName)] 
    public class RolesController : Controller
    {
        private readonly RoleManager<Role> _roleManager;
        private readonly UserManager<User> _userManager;

        public RolesController(RoleManager<Role> roleManager, UserManager<User> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }


        public IActionResult Index()
        {
            var roles = _roleManager.Roles.ToList();
            return View(roles);
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(Role role)
        {
            if (string.IsNullOrWhiteSpace(role.Name))
            {
                ModelState.AddModelError("Name", "Название роли не может быть пустым!");
                return View(role);
            }

            if (_roleManager.RoleExistsAsync(role.Name).Result)
            {
                ModelState.AddModelError("Name", "Роль с таким наименованием уже существует!");
                return View(role);
            }

            if (!ModelState.IsValid)
            {
                return View(role);
            }

            var result = _roleManager.CreateAsync(role).Result;
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(role);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Delete(int roleId)
        {
            var role = _roleManager.FindByIdAsync(roleId.ToString()).Result;
            if (role == null)
            {
                return RedirectToAction("Index");
            }

            var userInRole = _userManager.GetUsersInRoleAsync(role.Name).Result;
            if (userInRole.Any())
            {
                TempData["ErrorMessage"] = $"Нельзя удалить роль «{role.Name}», так как в ней находятся {userInRole.Count} пользователей.";
                return RedirectToAction("Index");
            }

            var result = _roleManager.DeleteAsync(role).Result;
            if (!result.Succeeded)
            {
                TempData["Error"] = $"Ошибка при удалении роли: {string.Join(", ", result.Errors.Select(e => e.Description))}";
            }

            return RedirectToAction("Index");
        }

    }
}
