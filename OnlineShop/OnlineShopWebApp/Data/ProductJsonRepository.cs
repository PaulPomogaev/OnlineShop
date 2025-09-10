using OnlineShopWebApp.Models;
using System.Text.Json;

namespace OnlineShopWebApp.Data
{
    public static class ProductJsonRepository
    {
        private static string _filePath = "Data/products.json";

        public static List<Product> GetAll()
        {
            if(!File.Exists(_filePath))
            {
                return new List<Product>();
            }

            var json = File.ReadAllText(_filePath);
            return JsonSerializer.Deserialize<List<Product>>(json) ?? new List<Product>();
        }

        public static void SaveAll(List<Product> products)
        {
            var json = JsonSerializer.Serialize(products, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_filePath, json);
        }

        public static void Add(Product product)
        {
            var products = GetAll();
            int newId = 1;
            if(products.Any())
            {
                newId = products.Max(p => p.Id) + 1;
            }
            product.Id = newId;
            products.Add(product);
            SaveAll(products);
        }

        public static Product ReturnById(int id)
        {
            var products = GetAll();
            return products.FirstOrDefault(p => p.Id == id);
        }
    }
}
