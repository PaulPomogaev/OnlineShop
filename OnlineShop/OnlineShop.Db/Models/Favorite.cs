namespace OnlineShop.Db.Models
{
    public class Favorite
    {
        public int Id { get; set; }
        public string UserId { get; set; } = "guest";
        public List<FavoriteItem> Items { get; set; } = new();

    }
}
