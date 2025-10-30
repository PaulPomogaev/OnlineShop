

namespace OnlineShopWebApp.Models
{
    public class OrderItemViewModel
    {
        public int Id { get; set; }
        public ProductViewModel Product { get; set; } = new()
        {
        Id = 0,
        Name = string.Empty,
        Cost = 0m
        };
        public int Quantity { get; set; }
        public decimal Cost => Product.Cost * Quantity;
    }
}
