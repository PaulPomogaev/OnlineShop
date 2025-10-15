using OnlineShopWebApp.Models;

namespace OnlineShopWebApp.Interfaces
{
    public interface IProductRepository
    {
        List<Product> GetAll();
        Product? GetById(int id);
        void Add(Product product);
        void Edit(Product product);
        void Delete(int id);
        List<Product> SearchEngine(string qwery);
    }
}
