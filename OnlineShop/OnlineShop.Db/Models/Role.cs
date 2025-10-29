using OnlineShop.Core.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace OnlineShop.Db.Models
{
    public class Role : IBaseId
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;
    }
}
