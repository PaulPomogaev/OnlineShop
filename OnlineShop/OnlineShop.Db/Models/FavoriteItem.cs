using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Db.Models
{
    public class FavoriteItem
    {
        public int Id { get; set; }
        public int FavoriteId { get; set; }
        public int ProductId { get; set; }
        public Favorite Favorite { get; set; }
        public Product Product { get; set; }
    }
}
