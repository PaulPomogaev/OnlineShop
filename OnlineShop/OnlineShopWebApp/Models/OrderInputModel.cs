namespace OnlineShopWebApp.Models
{
    public class OrderInputModel
    {
        public string CustomerName { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public DateOnly? DeliveryDate { get; set; }
        public string? Comment { get; set; }
    }
}
