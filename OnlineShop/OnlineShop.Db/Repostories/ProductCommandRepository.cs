using OnlineShop.Db.Interfaces;
using OnlineShop.Db.Models;

namespace OnlineShop.Db.Repostories
{
    public class ProductCommandRepository : IProductCommandRepository
    {
        private readonly DatabaseContext _context;

        public ProductCommandRepository(DatabaseContext context)
        {
            _context = context;
        }

        public void Add(Product product)
        {
            _context.Products.Add(product);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var product = _context.Products.FirstOrDefault(p => p.Id == id);

            if(product != null)
            {
                _context.Products.Remove(product);
                _context.SaveChanges();
            }
        }

        public void Edit(Product updateProduct)
        {
            var existingProduct = _context.Products.FirstOrDefault(p => p.Id == updateProduct.Id);

            if(existingProduct != null)
            {
                existingProduct.Name = updateProduct.Name;
                existingProduct.Cost = updateProduct.Cost;
                existingProduct.Description = updateProduct.Description;
                existingProduct.PhotoPath = updateProduct.PhotoPath;
                existingProduct.ImagePaths = updateProduct.ImagePaths;

                _context.SaveChanges();
            }
        }
    }
}
