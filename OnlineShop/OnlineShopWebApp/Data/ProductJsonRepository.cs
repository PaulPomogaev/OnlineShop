using OnlineShopWebApp.Models;
using System.Text.Json;

namespace OnlineShopWebApp.Data
{
    public static class ProductJsonRepository
    {
        private static string _filePath = "Data/products.json";
        private static int _nextId = 1;

        public static List<Product> GetAll()
        {
            if (!File.Exists(_filePath))
            {
                return new List<Product>();
            }

            var json = File.ReadAllText(_filePath);
            var products = JsonSerializer.Deserialize<List<Product>>(json) ?? new List<Product>();

            if (products.Any())
            {
                _nextId = products.Max(p => p.Id) + 1;
            }

            return products;
        }

        public static void SaveAll(List<Product> products)
        {
            var json = JsonSerializer.Serialize(products, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_filePath, json);
        }

        public static void Add(Product product)
        {

            product.Id = _nextId++;
            var products = GetAll();
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
