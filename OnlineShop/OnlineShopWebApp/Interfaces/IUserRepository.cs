using OnlineShopWebApp.Models;

namespace OnlineShopWebApp.Interfaces
{
    public interface IUserRepository
    {
        User? GetByLogin(string login);
        void Add(string login, string password);
        bool Exists(string login);
    }
}
