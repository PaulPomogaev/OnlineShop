using OnlineShop.Core.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace OnlineShop.Db.Models
{
    public class User : IBaseId
    {
        public int Id { get; set; }

        [Required]
        public string Login { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;
                
        public string? FirstName { get; set; } = string.Empty;
                
        public string? LastName { get; set; } = string.Empty;
                
        public string? Email { get; set; } = string.Empty;
                
        public string? Phone { get; set; } = string.Empty;

        public List<int> RoleIds { get; set; } = new List<int>();

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        //public List<Order> Orders { get; set; } = new List<Order>();
    }
}
