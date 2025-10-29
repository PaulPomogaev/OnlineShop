using OnlineShop.Db.Models;
using OnlineShop.Core.Interfaces;

namespace OnlineShop.Db.Interfaces
{
    public interface IRolesRepository : IRepository<Role>
    {
        bool Exist(string roleName);
    }
}
