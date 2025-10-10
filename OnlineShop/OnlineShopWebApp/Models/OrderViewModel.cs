namespace OnlineShopWebApp.Models
{
    public class OrderViewModel
    {
        public Order Order { get; set; } = new();
        public OrderInputModel InputModel { get; set; } = new();
    }
}
