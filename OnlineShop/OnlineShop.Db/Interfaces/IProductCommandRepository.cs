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
        void Add(Product product);
        void Edit(Product updateProduct);
        void Delete(int id);
    }
}
