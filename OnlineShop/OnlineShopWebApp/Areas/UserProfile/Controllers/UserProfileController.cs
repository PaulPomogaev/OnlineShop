using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Db.Models;
using OnlineShopWebApp.Models;
using Microsoft.AspNetCore.Identity;
using System.IO;
using OnlineShopWebApp.Controllers;
using OnlineShopWebApp.Helpers;

namespace OnlineShopWebApp.Areas.UserProfile.Controllers
{
    [Area("UserProfile")]
    [Authorize]
    public class UserProfileController : BaseController
    {
        private readonly UserManager<User> _userManager;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public UserProfileController(UserManager<User> userManager, IWebHostEnvironment webHostEnvironment)
        {
            _userManager = userManager;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Edit()
        {
            string userId = GetUserId();
            var user = _userManager.FindByNameAsync(userId).Result;
            if(user == null)
            {
                return RedirectToAction("Login", "Account", new { area = "" });
            }

            return View(user.ToEditModel());
        }

        [HttpPost]
        public IActionResult Edit(UserEdit model)
        {
            if(!ModelState.IsValid)
            {
                return View(model);
            }

            string currentUserName = GetUserId();
            var user = _userManager.FindByNameAsync(currentUserName).Result;

            if (user == null)
            {
                return RedirectToAction("Login", "Account", new { area = "" });
            }

            if(user.Id != model.Id)
            {
                return Forbid();
            }

            user.UpdateFromEditModel(model);

            if (model.AvatarFile != null)
            {
                var newAvatarPath = SaveAvatar(model.AvatarFile);
                DeleteAvatar(user.AvatarPath);
                user.AvatarPath = newAvatarPath;
            }

            var result = _userManager.UpdateAsync(user).Result;
            if(result.Succeeded)
            {
                TempData["SuccessMessage"] = "Профиль успешно обновлён.";
                return RedirectToAction("Index");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            model.AvatarPath = user.AvatarPath;

            return View(model);
        }
        
        private string? SaveAvatar(IFormFile avatarFile)
        {
            string avatarsPath = Path.Combine(_webHostEnvironment.WebRootPath, "images", "avatars");
            if (!Directory.Exists(avatarsPath))
            {
                Directory.CreateDirectory(avatarsPath);
            }

            var extension = Path.GetExtension(avatarFile.FileName); 
            var fileName = Guid.NewGuid() + extension; 
            var filePath = Path.Combine(avatarsPath, fileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                avatarFile.CopyTo(fileStream);
            }

            return $"/images/avatars/{fileName}";
        }

        private void DeleteAvatar(string? avatarPath)
        {
           if(string.IsNullOrEmpty(avatarPath) || avatarPath.StartsWith("/img/"))
           {
                return; 
           }

            var fullPath = Path.Combine(_webHostEnvironment.WebRootPath, avatarPath.TrimStart('/'));
            if (System.IO.File.Exists(fullPath))
            {
                System.IO.File.Delete(fullPath);
            }
        }

        [HttpPost]
        public IActionResult DeleteAvatar()
        {
            string currentUserName = GetUserId();
            var user = _userManager.FindByNameAsync(currentUserName).Result;

            if (user == null)
            {
                return RedirectToAction("Login", "Account", new { area = "" });
            }

            if (!string.IsNullOrEmpty(user.AvatarPath) && !user.AvatarPath.StartsWith("/img/default"))
            {
                DeleteAvatar(user.AvatarPath);
                user.AvatarPath = "/img/default-avatar.jpg";
                _userManager.UpdateAsync(user).Wait();

                TempData["SuccessMessage"] = "Аватар успешно удален!";
            }

            return RedirectToAction("Index");
        }
    }
}
