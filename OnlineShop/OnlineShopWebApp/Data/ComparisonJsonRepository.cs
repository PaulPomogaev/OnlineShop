using OnlineShopWebApp.Interfaces;
using OnlineShopWebApp.Models;
using System.Text;
using System.Text.Json;

namespace OnlineShopWebApp.Data
{
    public class ComparisonJsonRepository : IComparisonRepository
    {
        private readonly string _filepath = "Data/comparisons.json";
               
        private List<Comparison> GetAll()
        {
            if(!File.Exists(_filepath))
            {
                return new List<Comparison>();
            }

            var json = File.ReadAllText(_filepath, Encoding.UTF8);
            return JsonSerializer.Deserialize<List<Comparison>>(json) ?? new List<Comparison>();
        }

        private void SaveAll(List<Comparison> comparisons)
        {
            var json = JsonSerializer.Serialize(comparisons, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_filepath, json, Encoding.UTF8);
        }

        public void Add(int productId, string userId = "guest")
        {
            var comparisons = GetAll();
            var comparison = comparisons.FirstOrDefault(c => c.UserId == userId);

            if(comparison == null)
            {
                comparison = new Comparison { UserId = userId, ProductIds = new List<int>() };
                comparisons.Add(comparison);
            }

            if(!comparison.ProductIds.Contains(productId))
            {
                comparison.ProductIds.Add(productId);
                SaveAll(comparisons);
            }
        }

        public void Clear(string userId = "guest")
        {
            var comparisons = GetAll();
            var comparison = comparisons.FirstOrDefault(c => c.UserId == userId);

            if(comparison != null)
            {
                comparison.ProductIds.Clear();
                SaveAll(comparisons);
            }
        }

        public Comparison? Get(string userId = "guest")
        {
            var comparisons = GetAll();
            return comparisons.FirstOrDefault(c => c.UserId == userId);
        }

        public void Remove(int productId, string userId = "guest")
        {
            var comparisons = GetAll();
            var comparison = comparisons.FirstOrDefault(c => c.UserId == userId);

            if(comparison != null && comparison.ProductIds.Contains(productId))
            {
                comparison.ProductIds.Remove(productId);
                SaveAll(comparisons);
            }
        }

    }
}
