using OnlineShop.Db.Models;

namespace OnlineShopWebApp.Models
{
    public class UserRole
    {
        public int UserId { get; set; }
        public string UserLogin { get; set; } = string.Empty;
        public List<Role> AllRoles { get; set; } = new List<Role>();
        public List<int> UserRoleIds { get; set; } = new List<int>();
    }
}
