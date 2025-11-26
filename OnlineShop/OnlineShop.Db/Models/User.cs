using Microsoft.AspNetCore.Identity;
using OnlineShop.Core.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace OnlineShop.Db.Models
{
    public class User : IdentityUser<int>, IBaseId
    {
        [Required]
        public string Login { get; set; } = string.Empty;

       public string? FirstName { get; set; } = string.Empty;
                
        public string? LastName { get; set; } = string.Empty;
                
        public DateTime CreatedDate { get; set; } = DateTime.Now;
       
        public string? AvatarPath { get; set; }
    }
}
