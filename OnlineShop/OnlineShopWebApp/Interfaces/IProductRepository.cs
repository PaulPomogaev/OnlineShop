using OnlineShopWebApp.Models;

namespace OnlineShopWebApp.Interfaces
{
    public interface IProductRepository : IRepository<Product>
    {
        void Edit(Product product);
        List<Product> SearchEngine(string qwery);
    }
}
