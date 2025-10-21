using OnlineShopWebApp.Interfaces;
using OnlineShopWebApp.Models;
using System.Text;
using System.Text.Json;
using System.Security.Cryptography;

namespace OnlineShopWebApp.Data
{
    public class UserJsonRepository : BaseJsonRepository<User>, IUserRepository
    {
        public UserJsonRepository() : base("Data/users.json") { }
        
        public void Add(string login, string password)
        {
            var user = new User
            {
                Login = login,
                PasswordHash = HashPassword(password)
            };

            Add(user);
        }

        public User? GetByLogin(string login)
        {
            var users = GetAllInternal();
            return users.FirstOrDefault(u => u.Login.Equals(login, StringComparison.OrdinalIgnoreCase));
        }

        public bool Exists(string login)
        {
            return GetByLogin(login) != null;
        }

        public static string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }

        public static bool VerifyPassword(string password, string hash)
        {
            var hashOfInput = HashPassword(password);
            return hashOfInput == hash;
        }

        public void Update(User user)
        {
            var users = GetAllInternal();
            var existingUser = users.FirstOrDefault(u => u.Id == user.Id);
            
            if(existingUser != null)
            {
                existingUser.Login = user.Login;
                existingUser.FirstName = user.FirstName;
                existingUser.LastName = user.LastName;
                existingUser.Phone = user.Phone;
                SaveAll(users);
            }
        }

        public void UpdateProfile(int userId, string firstName, string lastName, string email, string phone)
        {
            var users = GetAllInternal();
            var user = users.FirstOrDefault(u => u.Id == userId);
            if (user != null)
            {
                user.FirstName = firstName;
                user.LastName = lastName;
                user.Login = email;
                user.Email = email;
                user.Phone = phone;
                SaveAll(users);
            }
        }

        public void ChangePassword(int userId, string oldPassword, string newPassword)
        {
            var users = GetAllInternal();
            var user = users.FirstOrDefault(u => u.Id == userId);
            if (user == null)
            {
                throw new InvalidOperationException("Пользователь не найден");
            }

            if(!VerifyPassword(oldPassword, user.PasswordHash))
            {
                throw new InvalidOperationException("Введён неверный старый пароль");
            }

            user.PasswordHash = HashPassword(newPassword);
            SaveAll(users);
        }

        public List<int> GetUserRoleIds(int userId)
        {
            var user = GetById(userId);
            if(user == null)
            {
                return new List<int>();
            }
            return user.RoleIds;
        }

        public void AssignRoles(int userId, List<int> roleIds)
        {
            var users = GetAllInternal();
            var user = users.FirstOrDefault(u => u.Id == userId);
            if (user != null)
            {
                if(roleIds == null)
                {
                    roleIds = new List<int>();
                }
                user.RoleIds = roleIds;
                SaveAll(users);
            }
        }

        public void AddFull(string login, string password, string firstName, string lastName, string email, string phone)
        {
            var user = new User
            {
                Login = login,
                PasswordHash = HashPassword(password),
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                Phone = phone,
                RoleIds = new List<int>()
            };
            Add(user);
        }
    }
}
