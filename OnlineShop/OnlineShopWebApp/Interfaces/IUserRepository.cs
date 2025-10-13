using OnlineShopWebApp.Models;

namespace OnlineShopWebApp.Interfaces
{
    public interface IUserRepository
    {
        User? GetByLogin(string login);
        void Add(User user);
        bool UserExists(string login);
    }
}
