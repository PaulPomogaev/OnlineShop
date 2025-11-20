

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
        public decimal Price { get; set; }  // покажет историю покупок по фиксированной цене на момент покупки
        public int Quantity { get; set; }
        public decimal Cost => Price * Quantity;
    }
}
