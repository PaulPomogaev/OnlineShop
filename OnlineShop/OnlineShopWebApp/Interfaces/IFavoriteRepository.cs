using OnlineShopWebApp.Models;

namespace OnlineShopWebApp.Interfaces
{
    public interface IFavoriteRepository
    {
        Favorite GetFavorite(string userId = "guest");
        void AddToFavorite(int productId, string userId = "guest");
        void RemoveFromFavorite(int productId, string userId = "guest");
        void ClearFavorite(string userId = "guest");
    }
}
