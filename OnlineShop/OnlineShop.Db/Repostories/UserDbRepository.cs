using OnlineShop.Db.Interfaces;
using OnlineShop.Db.Models;
using System.Text;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Identity;

namespace OnlineShop.Db.Repostories
{
    public class UserDbRepository : BaseDbRepository<User>, IUserRepository
    {
        private readonly UserManager<User> _userManager;

        public UserDbRepository(DatabaseContext context, UserManager<User> userManager) : base(context)
        {
            
            _userManager = userManager;
        }

        public void Add(string login, string password)
        {
            var user = new User
            {
                UserName = login,
                Login = login,
                Email = login, 
                EmailConfirmed = true,
                CreatedDate = DateTime.Now
            };

            var result = _userManager.CreateAsync(user, password).Result;
            if (!result.Succeeded)
            {
                throw new InvalidOperationException($"Ошибка при создании пользователя: {string.Join(", ", result.Errors)}");
            }
        }

        public User? GetByLogin(string login)
        {
            return _userManager.FindByNameAsync(login).Result;
        }

        public bool Exists(string login)
        {
            return GetByLogin(login) != null;
        }

       
        public void Edit(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (string.IsNullOrWhiteSpace(user.Login))
            {
                throw new ArgumentException("Логин не может быть пустым", nameof(user.Login));
            }

            var existingUser = _userManager.FindByIdAsync(user.Id.ToString()).Result;

            if (existingUser == null)
            {
                throw new InvalidOperationException($"Пользователь с ID {user.Id} не найден.");
            }    
                existingUser.Login = user.Login;
                existingUser.FirstName = user.FirstName;
                existingUser.LastName = user.LastName;
                existingUser.PhoneNumber = user.PhoneNumber;
                existingUser.Email = user.Login;
                existingUser.UserName = user.Login;

                var result = _userManager.UpdateAsync(existingUser).Result;
                if (!result.Succeeded)
                {
                    throw new InvalidOperationException($"Ошибка обновления данных пользователя: {string.Join(", ", result.Errors)}");
                }
            
        }

        public void UpdateProfile(int userId, string firstName, string lastName, string email, string phone)
        {
            var user = _userManager.FindByIdAsync(userId.ToString()).Result;
            if (user == null)
            {
                throw new InvalidOperationException($"Пользователь с ID {userId} не найден.");
            }
                        
                user.FirstName = firstName;
                user.LastName = lastName;
                user.Login = email;
                user.Email = email;
                user.UserName = email;
                user.PhoneNumber = phone; 
                user.EmailConfirmed = true;

                var result = _userManager.UpdateAsync(user).Result;
                if (!result.Succeeded)
                {
                    throw new InvalidOperationException($"Ошибка обновления профиля: {string.Join(", ", result.Errors)}");
                }
            
        }

        public void ChangePassword(int userId, string oldPassword, string newPassword)
        {
            var user = _userManager.FindByIdAsync(userId.ToString()).Result;
            if (user == null)
            {
                throw new InvalidOperationException("Пользователь не найден");
            }

            var result = _userManager.ChangePasswordAsync(user, oldPassword, newPassword).Result;
            if (!result.Succeeded)
            {
                throw new InvalidOperationException($"Ошибка при смене пароля: {string.Join(", ", result.Errors)}");
            }
        }
          

        public void AddFull(UserCreationData data)
        {
            var user = new User
            {
                UserName = data.Email,          
                Login = data.Email,            
                Email = data.Email,
                EmailConfirmed = true,     
                PhoneNumber = data.Phone,
                FirstName = data.FirstName,
                LastName = data.LastName,
                CreatedDate = DateTime.Now,
            };
            var result = _userManager.CreateAsync(user, data.Password).Result;
            if (!result.Succeeded)
            {
                throw new InvalidOperationException($"Ошибка при создании пользователя: {string.Join(", ", result.Errors)}");
            }
        }

        public List<string> GetUserRoles(int userId)
        {
            var user = _userManager.FindByIdAsync(userId.ToString()).Result;
            if (user == null)
            {
                return new List<string>();
            }    
            return _userManager.GetRolesAsync(user).Result.ToList();
        }

        public List<int> GetUserRoleIds(int userId)
        {
            var user = _userManager.FindByIdAsync(userId.ToString()).Result;
            if (user == null)
            {
                return new List<int>();
            }    
            
            var roleNames = _userManager.GetRolesAsync(user).Result;

            
            var allRoles = _context.Roles.ToList(); 

            return allRoles.Where(r => roleNames.Contains(r.Name)).Select(r => r.Id).ToList();

        }

        public void AssignRoles(int userId, List<int> roleIds)
        {
            var user = _userManager.FindByIdAsync(userId.ToString()).Result;
            if (user == null)
                return;

            var currentRoles = _userManager.GetRolesAsync(user).Result;
            _userManager.RemoveFromRolesAsync(user, currentRoles).Wait();

            if (roleIds?.Any() == true)
            {
                var roleNames = _context.Roles.Where(r => roleIds.Contains(r.Id)).Select(r => r.Name).ToList();

                _userManager.AddToRolesAsync(user, roleNames).Wait();

            }
        }
    }
}
