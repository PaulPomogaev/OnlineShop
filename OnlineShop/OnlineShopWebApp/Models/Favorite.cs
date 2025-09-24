namespace OnlineShopWebApp.Models
{
    public class Favorite
    {
        public string UserId { get; set; } = "guest";
        public List<int> ProductIds { get; set; } = new();
    }
}
