using OnlineShop.Db.Models;

namespace OnlineShopWebApp.Models

{
    public class CartItemViewModel
    {
        public int Id { get; set; } // идентификатор элемента корзины
        public required ProductViewModel Product { get; set; }
        public int Quantity { get; set; }
        public decimal Cost => Product.Cost * Quantity; // всегда будет актуальной
    }
}
