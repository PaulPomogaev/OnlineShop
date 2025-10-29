namespace OnlineShopWebApp.Models
{
    public class CartViewModel
    {
        public int Id { get; set; } // идентификатор корзины
        public string UserId { get; set; } = "guest"; // идентификатор пользователя
        public List<CartItem> Items { get; set; } = new();
        public decimal TotalCost => Items.Sum(item => item.Cost);
    }
}
