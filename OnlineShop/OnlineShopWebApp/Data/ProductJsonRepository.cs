using OnlineShopWebApp.Controllers;
using OnlineShopWebApp.Interfaces;
using OnlineShopWebApp.Models;
using System.Text;
using System.Text.Json;

namespace OnlineShopWebApp.Data
{
    public class ProductJsonRepository : IProductRepository
    {
        private readonly string _filePath = "Data/products.json";
        private int _nextId;

        public ProductJsonRepository()
        {
            InitializeNextId();
        }

        private void InitializeNextId()
        {
            if (!File.Exists(_filePath))
            {
                _nextId = 1;
                return;
            }

            var json = File.ReadAllText(_filePath, Encoding.UTF8);
            var products = JsonSerializer.Deserialize<List<Product>>(json) ?? new List<Product>();

            _nextId = products.Any() ? products.Max(p => p.Id) + 1 : 1;

        }

        public List<Product> GetAll()
        {
            if (!File.Exists(_filePath))
            {
                return new List<Product>();
            }

            var json = File.ReadAllText(_filePath, Encoding.UTF8);
            return JsonSerializer.Deserialize<List<Product>>(json) ?? new List<Product>();

        }


        private void SaveAll(List<Product> products)
        {
            var json = JsonSerializer.Serialize(products, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_filePath, json, Encoding.UTF8);
        }

        public void Add(Product product)
        {

            product.Id = _nextId++;
            var products = GetAll();
            products.Add(product);
            SaveAll(products);
        }

        public void Edit(Product updateProduct)
        {
            var products = GetAll();
            var existingProduct = products.FirstOrDefault(p => p.Id == updateProduct.Id);

            if (existingProduct != null)
            {
                existingProduct.Name = updateProduct.Name;
                existingProduct.Cost = updateProduct.Cost;
                existingProduct.Description = updateProduct.Description;
                SaveAll(products);
            }
        }

        public void Delete(int id)
        {
            var products = GetAll();
            var productToRemove = products.FirstOrDefault(p => p.Id == id);

            if (productToRemove != null)
            {
                products.Remove(productToRemove);
                SaveAll(products);
            }
        }

        public List<Product> SearchEngine(string query)
        {
            if(string.IsNullOrWhiteSpace(query))
            {
                return GetAll();
            }

            var allProducts = GetAll();
            return allProducts.Where(p => p.Name.Contains(query, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        public Product? GetById(int id)
        {
            var products = GetAll();
            return products.FirstOrDefault(p => p.Id == id);
        }
    }
}