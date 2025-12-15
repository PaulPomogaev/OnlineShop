namespace OnlineShop.Core.Models.Products
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Cost { get; set; }
        public string? Description { get; set; }
        public string? PhotoPath { get; set; }
        public List<string>? ImagePaths { get; set; }
    }
}
