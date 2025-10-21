using OnlineShopWebApp.Interfaces;
using OnlineShopWebApp.Models;
using System.Text.Json;
using System.Text;

namespace OnlineShopWebApp.Data
{
    public class RolesJsonRepository : BaseJsonRepository<Role>, IRolesRepository
    {
        public RolesJsonRepository() : base("Data/roles.json") { }

        public bool Exist(string roleName)
        {
            var roles = GetAllInternal();
            return roles.Any(r => r.Name.Equals(roleName, StringComparison.OrdinalIgnoreCase));
        }

    }
}
