using OnlineShop.Db.Interfaces;
using OnlineShop.Db.Models;

namespace OnlineShop.Db.Repostories
{
    public class RolesDbRepository : BaseDbRepository<Role>, IRolesRepository
    {
        private readonly DatabaseContext _context;

        public RolesDbRepository(DatabaseContext context) : base(context)
        {
            _context = context;
        }

        public bool Exist(string roleName)
        {
            return _context.Roles.Any(r => r.Name.Equals(roleName, StringComparison.OrdinalIgnoreCase));
        }
    }
}
