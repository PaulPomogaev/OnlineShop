using OnlineShop.Db.Models;
using OnlineShop.Core.Interfaces;
using OnlineShop.Core.Models;

namespace OnlineShop.Db.Interfaces
{
    public interface IFavoriteRepository
    {
        Favorite? Get(string userId = "guest");
        void Add(int productId, string userId = "guest");
        void Remove(int productId, string userId = "guest");
        void Clear(string userId = "guest");
    }
}
