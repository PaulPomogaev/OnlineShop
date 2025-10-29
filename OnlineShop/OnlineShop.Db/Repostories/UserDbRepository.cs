using OnlineShop.Db.Interfaces;
using OnlineShop.Db.Models;
using System.Text;
using System.Security.Cryptography;

namespace OnlineShop.Db.Repostories
{
    public class UserDbRepository : BaseDbRepository<User>, IUserRepository
    {
        private readonly DatabaseContext _context;

        public UserDbRepository(DatabaseContext context) : base(context)
        {
            _context = context;
        }

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
            return _context.Users.FirstOrDefault(u =>
                u.Login.Equals(login, StringComparison.OrdinalIgnoreCase));
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
            var existingUser = _context.Users.FirstOrDefault(u => u.Id == user.Id);

            if (existingUser != null)
            {
                existingUser.Login = user.Login;
                existingUser.FirstName = user.FirstName;
                existingUser.LastName = user.LastName;
                existingUser.Phone = user.Phone;
                _context.SaveChanges();
            }
        }

        public void UpdateProfile(int userId, string firstName, string lastName, string email, string phone)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == userId);
            if (user != null)
            {
                user.FirstName = firstName;
                user.LastName = lastName;
                user.Login = email;
                user.Email = email;
                user.Phone = phone;
                _context.SaveChanges();
            }
        }

        public void ChangePassword(int userId, string oldPassword, string newPassword)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == userId);
            if (user == null)
            {
                throw new InvalidOperationException("Пользователь не найден");
            }

            if (!VerifyPassword(oldPassword, user.PasswordHash))
            {
                throw new InvalidOperationException("Введён неверный старый пароль");
            }

            user.PasswordHash = HashPassword(newPassword);
            _context.SaveChanges();
        }

        public List<int> GetUserRoleIds(int userId)
        {
            var user = GetById(userId);
            if (user == null)
            {
                return new List<int>();
            }
            return user.RoleIds;
        }

        public void AssignRoles(int userId, List<int> roleIds)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == userId);
            if (user != null)
            {
                user.RoleIds = roleIds ?? new List<int>();
                _context.SaveChanges();
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
