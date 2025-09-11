using OnlineShopWebApp.Models;
using System.Text.Json;

namespace OnlineShopWebApp.Data
{
    public static class ProductJsonRepository
    {
        private static string _filePath = "Data/products.json";
        private static int _nextId;

        static ProductJsonRepository()
        {
            InitializeNextId();
        }

        private static void InitializeNextId()
        {
            if(!File.Exists(_filePath))
            {
                _nextId = 1;
                return;
            }

            var json = File.ReadAllText(_filePath);
            var products = JsonSerializer.Deserialize<List<Product>>(json) ?? new List<Product>();

            _nextId = products.Any() ? products.Max(p => p.Id) + 1 : 1;

        }

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
            
            product.Id = _nextId++;
            var products = GetAll();
            products.Add(product);
            SaveAll(products);
        }
    }
}
