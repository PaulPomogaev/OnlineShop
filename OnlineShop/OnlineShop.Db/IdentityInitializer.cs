using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.VisualBasic;
using OnlineShop.Db.Models;

namespace OnlineShop.Db
{
    public static class IdentityInitializer
    {
        public static void Initialize(UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            var adminEmail = "admin@example.com";
            var password = "AdminPassword123!";
            var adminRoleName = Constants.AdminRoleName; 
            var userRoleName = Constants.UserRoleName;


            if (roleManager.FindByNameAsync(adminRoleName).Result == null)
            {
                roleManager.CreateAsync(new Role { Name = adminRoleName }).Wait();
            }

            
            if (roleManager.FindByNameAsync(userRoleName).Result == null)
            {
                roleManager.CreateAsync(new Role { Name = userRoleName }).Wait();
            }

            
            if (userManager.FindByEmailAsync(adminEmail).Result == null)
            {
                var admin = new User
                {
                    UserName = adminEmail,  
                    Email = adminEmail,
                    EmailConfirmed = true,
                    Login = "admin",         
                    FirstName = "Админ",
                    LastName = "Админов",
                    CreatedDate = DateTime.Now
                };

                var result = userManager.CreateAsync(admin, password).Result;
                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(admin, adminRoleName).Wait();
                }
            }
        }
    }
}
