namespace OnlineShopWebApp.Models
{
    public class Product
    {
        public string PhotoPath { get; set; } = "img/whey-protein.jpg";
        public required int Id { get; set; }
        public required string Name { get; set; }
        public required decimal Cost { get; set; }
        public  string? Description { get; set; }
               
    }
}
