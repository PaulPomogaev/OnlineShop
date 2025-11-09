using OnlineShop.Db.Models;
using OnlineShop.Core.Interfaces;

namespace OnlineShop.Db.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        User? GetByLogin(string login);
        void Add(string login, string password);
        bool Exists(string login);
        void Edit(User user);
        void UpdateProfile(int userId, string firstName, string lastName, string email, string phone);
        void ChangePassword(int userId, string oldPassword, string newPassword);
        void AddFull(string login, string password, string firstName, string lastName, string email, string phone);
        List<string> GetUserRoles(int userId);
        List<int> GetUserRoleIds(int userId);    
        void AssignRoles(int userId, List<int> roleIds);
    }
}
