namespace OnlineShopWebApp.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string UserId { get; set; } = "guest";
        public List<OrderItem> Items { get; set; } = new();
        public decimal TotalCost => Items.Sum(item => item.Cost);
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public CustomerInfo Customer { get; set; } = new();
    }
}
