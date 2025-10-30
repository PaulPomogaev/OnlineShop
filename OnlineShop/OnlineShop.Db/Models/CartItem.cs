using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineShop.Db.Models
{
    public class CartItem
    {
        public int Id { get; set; } // идентификатор элемента корзины
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public Cart Cart { get; set; }
        public int Quantity { get; set; }
                
    }
}
