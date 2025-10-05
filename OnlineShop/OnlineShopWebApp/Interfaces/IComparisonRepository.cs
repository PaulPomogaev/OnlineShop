using OnlineShopWebApp.Models;

namespace OnlineShopWebApp.Interfaces
{
    public interface IComparisonRepository
    {
        Comparison? Get(string userId = "guest");
        void Add(int productId, string userId = "guest");
        void Remove(int productId, string userId = "guest");
        void Clear(string userId = "guest");
    }
}
