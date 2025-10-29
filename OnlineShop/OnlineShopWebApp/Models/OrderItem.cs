using OnlineShop.Db.Models;

namespace OnlineShopWebApp.Models
{
    public class OrderItem
    {
        public int Id { get; set; }
        public Product Product { get; set; } 
        public int Quantity { get; set; }
        public decimal Cost => Product.Cost * Quantity;
    }
}
