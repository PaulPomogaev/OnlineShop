namespace OnlineShopWebApp.Models
{
    public class CartItem
    {
        public int Id { get; set; } // идентификатор элемента корзины
        public required Product Product { get; set; }
        public int Quantity { get; set; }
        public decimal Cost => Product.Cost * Quantity; // всегда будет актуальной
    }
}
