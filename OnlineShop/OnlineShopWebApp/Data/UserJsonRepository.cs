using OnlineShopWebApp.Interfaces;
using OnlineShopWebApp.Models;
using System.Text;
using System.Text.Json;
using System.Security.Cryptography;

namespace OnlineShopWebApp.Data
{
    public class UserJsonRepository : IUserRepository
    {
        private readonly string _filepath = "Data/users.json";
        private int _nextId;

        public UserJsonRepository()
        {
            InitializeNextIds();
        }

        public void InitializeNextIds()
        {
            var users = GetAll();
            _nextId = users.Any() ? users.Max(u => u.Id) + 1 : 1;
        }

        public List<User> GetAll()
        {
            if(!File.Exists(_filepath))
            {
                return new List<User>();
            }

            var json = File.ReadAllText(_filepath, Encoding.UTF8);
            return JsonSerializer.Deserialize<List<User>>(json) ?? new List<User>();
        }

        private void SaveAll(List<User> users)
        {
            var json = JsonSerializer.Serialize(users, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_filepath, json, Encoding.UTF8);
        }

        public void Add(string login, string password)
        {
            var user = new User
            {
                Id = _nextId++,
                Login = login,
                PasswordHash = HashPassword(password)
            };

            var users = GetAll();
            users.Add(user);
            SaveAll(users);
        }

        public User? GetByLogin(string login)
        {
            var users = GetAll();
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

        public User? GetById(int id)
        {
            var users = GetAll();
            return users.FirstOrDefault(u => u.Id == id);
        }

        public void Update(User user)
        {
            var users = GetAll();
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

        public void Delete(int id)
        {
            var users = GetAll();
            var user = users.FirstOrDefault(u => u.Id == id);

            if (user != null)
            {
                users.Remove(user);
                SaveAll(users);
            }

        }

        public void UpdateProfile(int userId, string firstName, string lastName, string email, string phone)
        {
            var users = GetAll();
            var user = users.FirstOrDefault(u => u.Id == userId);
            if (user != null)
            {
                user.FirstName = firstName;
                user.LastName = lastName;
                user.Email = email;
                user.Phone = phone;
                SaveAll(users);
            }
        }

        public void ChangePassword(int userId, string oldPassword, string newPassword)
        {
            var users = GetAll();
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
            var users = GetAll();
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
                Id = _nextId++,
                Login = login,
                PasswordHash = HashPassword(password),
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                Phone = phone,
                RoleIds = new List<int>()
            };
            var users = GetAll();
            users.Add(user);
            SaveAll(users);
        }
    }
}
