using OnlineShopWebApp.Models;

namespace OnlineShopWebApp.Interfaces
{
    public interface IRolesRepository : IRepository<Role>
    {
        bool Exist(string roleName);
    }
}
