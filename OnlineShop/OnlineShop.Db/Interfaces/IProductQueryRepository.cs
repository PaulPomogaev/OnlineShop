using OnlineShop.Db.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineShop.Db.Interfaces
{
    public interface IProductQueryRepository
    {
        List<Product> GetAll();
        Product? GetById(int id);
        Task<Product?> GetByIdAsync(int id);
        List<Product> SearchEngine(string qwery);
    }
}
