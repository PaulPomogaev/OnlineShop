using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineShop.Db.Models
{
    public class Cart
    {
        public int Id { get; set; } // идентификатор корзины
        public string UserId { get; set; } = "guest"; // идентификатор пользователя
        public List<CartItem> Items { get; set; } = new();
                
    }
}
