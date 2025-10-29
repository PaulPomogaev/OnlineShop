using Microsoft.AspNetCore.Mvc;
using OnlineShopWebApp.Interfaces;
using OnlineShopWebApp.Models;
using System;
using OnlineShop.Db.Models;
using OnlineShop.Db.Interfaces;

namespace OnlineShopWebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UsersController : Controller
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        public IActionResult Index()
        {
            var users = _userService.GetAllUsers();
            return View(users);
        }

        public IActionResult Detail(int id)
        {
            try
            {
                var model = _userService.GetUserDetails(id);
                return View(model);
            }
            catch (InvalidOperationException)
            {
                return NotFound();
            }
        }

        public IActionResult Edit(int id)
        {
            var model = _userService.GetUserEditModel(id);
            if(model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(UserEdit model)
        {
            if(!ModelState.IsValid)
            {
                return View(model);
            }

            _userService.UpdateUserProfile(model);
            return RedirectToAction("Index");
        }

        public IActionResult ChangePassword(int id)
        {
            var model = _userService.GetChangePasswordModel(id);
            return View(model);
        }

        [HttpPost]
        public IActionResult ChangePassword(ChangePassword model)
        {
            if(!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                _userService.ChangeUserPassword(model);
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError("OldPassword", ex.Message);
                return View(model);
            }
            return RedirectToAction("Detail", new { id = model.UserId });
        }


        public IActionResult AssignRoles(int id)
        {
            var model = _userService.GetUserRoleModel(id);
            if(model == null)
            {
                return NotFound();
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult AssignRoles(UserRole model)
        {
            _userService.AssignUserRoles(model);
            return RedirectToAction("Detail", new { id = model.UserId });
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            _userService.DeleteUser(id);
            return RedirectToAction("Index");
        }

        public IActionResult Create()
        {
            var model = _userService.GetUserCreateModel();
            return View(model);
        }

        [HttpPost]
        public IActionResult Create(UserCreate model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                _userService.CreateUser(model);
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError("Login", ex.Message);
                return View(model);
            }
            return RedirectToAction("Index");
        }
    }
}
