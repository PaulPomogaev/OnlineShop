using OnlineShop.Db.Models;
using OnlineShop.Core.Interfaces;

namespace OnlineShop.Db.Interfaces
{
    public interface IProductRepository : IRepository<Product>
    {
        void Edit(Product product);
        List<Product> SearchEngine(string qwery);

        Task<Product?> GetByIdAsync(int id);
    }
}
