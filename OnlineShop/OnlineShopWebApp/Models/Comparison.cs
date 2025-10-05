namespace OnlineShopWebApp.Models
{
    public class Comparison
    {
        public string UserId { get; set; } = "guest";
        public List<int> ProductIds { get; set; } = new();
    }
}
