using OnlineShopWebApp.Models;

namespace OnlineShopWebApp.Interfaces
{
    public interface ICartRepository
    {
        Cart GetCart(string userId = "guest");
        void AddToCart(int productId, int quantity = 1, string userId = "guest");
        void RemoveFromCart(int itemId, string userId = "guest");
        void ClearCart(string userId = "guest");
        void IncreaseItemQuantity(int itemId, string userId = "guest");
        void DecreaseItemQuantity(int itemId, string userId = "guest");
    }
}
