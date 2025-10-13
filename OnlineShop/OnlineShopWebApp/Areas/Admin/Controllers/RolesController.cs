using Microsoft.AspNetCore.Mvc;
using OnlineShopWebApp.Interfaces;
using OnlineShopWebApp.Models;

namespace OnlineShopWebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RolesController : Controller
    {
        private readonly IRolesRepository _rolesRepository;

        public RolesController(IRolesRepository rolesRepository)
        {
            _rolesRepository = rolesRepository;
        }
        

        public IActionResult Roles()
        {
            var roles = _rolesRepository.GetAll();
            return View(roles);
        }

        public IActionResult AddRole()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddRole(Role role)
        {
            if (_rolesRepository.Exist(role.Name))
            {
                ModelState.AddModelError("Name", "Роль с таким наименованием уже существует!");
                return View(role);
            }

            if (!ModelState.IsValid)
            {
                return View(role);
            }

            _rolesRepository.Add(role);


            return RedirectToAction("Roles");
        }

        [HttpPost]
        public IActionResult DeleteRole(int roleId)
        {
            _rolesRepository.Delete(roleId);
            return RedirectToAction("Roles");
        }

    }
}
