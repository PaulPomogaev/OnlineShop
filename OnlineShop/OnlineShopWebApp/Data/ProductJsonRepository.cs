using OnlineShopWebApp.Controllers;
using OnlineShopWebApp.Interfaces;
using OnlineShopWebApp.Models;
using System.Text;
using System.Text.Json;

namespace OnlineShopWebApp.Data
{
    public class ProductJsonRepository : BaseJsonRepository<Product>, IProductRepository
    {
       public ProductJsonRepository() : base("Data/products.json") { }
        
                
        public void Edit(Product updateProduct)
        {
            var products = GetAllInternal();
            var existingProduct = products.FirstOrDefault(p => p.Id == updateProduct.Id);

            if (existingProduct != null)
            {
                existingProduct.Name = updateProduct.Name;
                existingProduct.Cost = updateProduct.Cost;
                existingProduct.Description = updateProduct.Description;
                SaveAll(products);
            }
        }

        
        public List<Product> SearchEngine(string query)
        {
            if(string.IsNullOrWhiteSpace(query))
            {
                return GetAllInternal();
            }

            var allProducts = GetAllInternal();
            return allProducts.Where(p => p.Name.Contains(query, StringComparison.OrdinalIgnoreCase)).ToList();
        }
                
    }
}