namespace OnlineShopWebApp.Models
{
    public class Cart
    {
        public List<CartItem> Items { get; set; } = new();
        public decimal TotalCost => Items.Sum(item => item.Cost);
    }
}
