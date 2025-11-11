using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Db;
using OnlineShop.Db.Models;
using OnlineShopWebApp.Models;
using System.Data;


namespace OnlineShopWebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = Constants.AdminRoleName)]
    public class UsersController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;

        public UsersController(UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public IActionResult Index()
        {
            var users = _userManager.Users.ToList();
            return View(users);
        }

        public IActionResult Detail(int id)
        {
            var user = _userManager.FindByIdAsync(id.ToString()).Result;
            if(user == null)
            {
                return NotFound();
            }

            var roles = _userManager.GetRolesAsync(user).Result;
            var model = new UserDetails
            {
                Id = user.Id,
                Login = user.Login,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                CreatedDate = user.CreatedDate,
                RoleNames = roles.ToList()
            };

            return View(model);
        }

        public IActionResult Edit(int id)
        {
            var user = _userManager.FindByIdAsync(id.ToString()).Result;
            if (user == null)
            {
                return NotFound();
            }

            var model = new UserEdit
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(UserEdit model)
        {
            if(!ModelState.IsValid)
            {
                return View(model);
            }

            var user = _userManager.FindByIdAsync(model.Id.ToString()).Result;
            if (user == null)
            {
                return NotFound();
            }

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Email = model.Email;
            user.PhoneNumber = model.PhoneNumber;
            user.UserName = model.Email;

            var result = _userManager.UpdateAsync(user).Result;
            if(result.Succeeded)
            {
                return RedirectToAction("Index");
            }

            foreach(var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }

        public IActionResult ChangePassword(int id)
        {
            var user = _userManager.FindByIdAsync(id.ToString()).Result;
            if (user == null)
            {
                return NotFound();
            }
            return View(new ChangePassword { UserId = id});
        }

        [HttpPost]
        public IActionResult ChangePassword(ChangePassword model)
        {
            if(!ModelState.IsValid)
            {
                return View(model);
            }

            var user = _userManager.FindByIdAsync(model.UserId.ToString()).Result;
            if (user == null)
            {
                return NotFound();
            }

            var result = _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword).Result;

            if(result.Succeeded)
            {
                return RedirectToAction("Detail", new { id = model.UserId });
            }
            ModelState.AddModelError("OldPassword", "Неверный старый пароль");

            return View(model);
        }


        public IActionResult AssignRoles(int id)
        {
            var user = _userManager.FindByIdAsync(id.ToString()).Result;
            if (user == null)
            {
                return NotFound();
            }

            var allRoles = _roleManager.Roles.ToList();
            var userRoles = _userManager.GetRolesAsync(user).Result;
            var userRoleIds = allRoles.Where(r => userRoles.Contains(r.Name)).Select(r => r.Id).ToList();

            var model = new UserRole
            {
                UserId = id,
                UserLogin = user.Login,
                AllRoles = allRoles,
                UserRoleIds = userRoleIds
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult AssignRoles(UserRole model)
        {
            var user = _userManager.FindByIdAsync(model.UserId.ToString()).Result;
            if(user == null)
            {
                return NotFound();
            }

            var currentRoles = _userManager.GetRolesAsync(user).Result;
            _userManager.RemoveFromRolesAsync(user, currentRoles).Wait();

            var selectedRoles = _roleManager.Roles.Where(r => model.UserRoleIds.Contains(r.Id)).Select(r => r.Name).ToList();

            if(selectedRoles.Any())
            {
                _userManager.AddToRolesAsync(user, selectedRoles).Wait();
            }

            return RedirectToAction("Detail", new { id = model.UserId });
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var user = _userManager.FindByIdAsync(id.ToString()).Result;
            if(user == null)
            {
                return NotFound();
            }
            var result = _userManager.DeleteAsync(user).Result;

            if (!result.Succeeded)
            {
               return RedirectToAction("Detail", new { id });
            }

            return RedirectToAction("Index");
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(UserCreate model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (_userManager.FindByNameAsync(model.Login).Result != null)
            {
                ModelState.AddModelError("Login", "Пользователь с таким логином уже существует");
                return View(model);
            }

            var user = new User
            {
                UserName = model.Login,
                Login = model.Login,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                PhoneNumber = model.PhoneNumber,
                CreatedDate = DateTime.Now
            };

            var result = _userManager.CreateAsync(user, model.Password).Result;
            if (result.Succeeded)
            {
                    if (_roleManager.RoleExistsAsync("User").Result)
                    _userManager.AddToRoleAsync(user, "User").Wait();

                return RedirectToAction("Index");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            
            return View(model);
        }
    }
}
