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
    }
}
