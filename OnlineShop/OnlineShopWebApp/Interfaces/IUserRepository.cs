using OnlineShopWebApp.Models;

namespace OnlineShopWebApp.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        User? GetByLogin(string login);
        void Add(string login, string password);
        bool Exists(string login);
        void Update(User user);
        void UpdateProfile(int userId, string firstName, string lastName, string email, string phone);
        void ChangePassword(int userId, string oldPassword, string newPassword);
        List<int> GetUserRoleIds(int userId);
        void AssignRoles(int userId, List<int> roleIds);
        void AddFull(string login, string password, string firstName, string lastName, string email, string phone);
    }
}
