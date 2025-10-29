using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineShop.Db.Models
{
    public class OrderItem
    {
        public int Id { get; set; }
        public Product? Product { get; set; } 
        public int Quantity { get; set; }

        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public Order? Order { get; set; }

        [NotMapped]
        public decimal Cost => Product.Cost * Quantity; // нужно для отображения страницы "Детали заказа"
    }
}
