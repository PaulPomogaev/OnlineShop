using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Core.Interfaces;
using OnlineShop.Db.Interfaces;
using OnlineShop.Db.Models;
using OnlineShopWebApp.Models;
using OnlineShopWebApp.Services;

namespace OnlineShopWebApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ICartRepository _cartRepository;
        private readonly IFavoriteRepository _favoriteRepository; 
        private readonly IComparisonRepository _comparisonRepository;
        
        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, ICartRepository cartRepository, IFavoriteRepository favoriteRepository, IComparisonRepository comparisonRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _cartRepository = cartRepository;
            _favoriteRepository = favoriteRepository;
            _comparisonRepository = comparisonRepository;
            
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                return RedirectToAction("Index", "Home"); 
            }
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginModel model, string returnUrl = null)
        {
            if(!ModelState.IsValid)
            {
                return View(model);
            }

            var user = _userManager.FindByNameAsync(model.Login).Result;
            if(user == null)
            {
                ModelState.AddModelError("", "Неверный логин или пароль");
                return View(model);
            }

            var result = _signInManager.PasswordSignInAsync(
                user, model.Password, model.RememberMe, lockoutOnFailure: false).Result;

            if (result.Succeeded)
            {
                var migrator = new GuestMigrator(
                    _cartRepository,
                    _favoriteRepository,
                    _comparisonRepository,
                    HttpContext);
                migrator.MigrateGuestData(user.UserName);

                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }

                return RedirectToAction("Index", "Home");
            }
                       

            ModelState.AddModelError("", "Неверный логин или пароль");

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            _signInManager.SignOutAsync().Wait();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Register(string returnUrl = null)
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        public IActionResult Register(RegisterModel model, string returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                ViewData["ReturnUrl"] = returnUrl;
                return View(model);
            }

            if(_userManager.FindByNameAsync(model.Login).Result != null)
            {
                ModelState.AddModelError("Login", "Пользователь с таким логином уже существует");
                ViewData["ReturnUrl"] = returnUrl;
                return View(model);
            }

            var user = new User
            {
                UserName = model.Login,   
                Login = model.Login,      
                Email = model.Login,      
                CreatedDate = DateTime.Now
            };

            var result = _userManager.CreateAsync(user, model.Password).Result;

            if (result.Succeeded)
            {
                var roleManager = HttpContext.RequestServices.GetRequiredService<RoleManager<Role>>();
                if (roleManager.FindByNameAsync("User").Result != null)
                {
                    _userManager.AddToRoleAsync(user, "User").Wait();
                }

                _signInManager.SignInAsync(user, isPersistent: false).Wait();

                var migrator = new GuestMigrator(
                    _cartRepository,
                    _favoriteRepository,
                    _comparisonRepository,
                    HttpContext);
                migrator.MigrateGuestData(user.UserName);

                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }

                return RedirectToAction("Index", "Home");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            ViewData["ReturnUrl"] = returnUrl;
            return View(model);
        }

    }
}
