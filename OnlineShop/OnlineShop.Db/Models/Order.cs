using OnlineShop.Core.Interfaces;
using OnlineShop.Core.Models;
using System.ComponentModel.DataAnnotations.Schema;


namespace OnlineShop.Db.Models
{
    public class Order : IBaseId
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string UserId { get; set; } = "guest";
         public List<OrderItem> Items { get; set; } = new();
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateOnly? DeliveryDate { get; set; }
        public string? Comment { get; set; }

        public CustomerInfo Customer { get; set; } = new();
        public OrderStatus Status { get; set; } = OrderStatus.Created;
       
    }
}
