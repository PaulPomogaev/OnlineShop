using OnlineShopWebApp.Models;

namespace OnlineShopWebApp.Interfaces
{
    public interface IRolesRepository
    {
        List<Role> GetAll();
        void Add(Role role);
        Role? GetById(int id);
        void Delete(int id);
        bool Exist(string roleName);
    }
}
