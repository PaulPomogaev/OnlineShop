using OnlineShop.Db.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Db.Interfaces
{
    public interface IProductCommandRepository
    {
        Task AddAsync(Product product);
        Task EditAsync(Product updateProduct);
        Task DeleteAsync(int id);
    }
}
