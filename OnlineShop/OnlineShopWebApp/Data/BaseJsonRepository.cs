using OnlineShopWebApp.Interfaces;
using OnlineShopWebApp.Models;
using System.Text.Json;
using System.Text;

namespace OnlineShopWebApp.Data
{
    public abstract class BaseJsonRepository<T> : IRepository<T> where T : class, IBaseId
    {
        protected readonly string FilePath;
        
        protected BaseJsonRepository(string filePath)
        {
            FilePath = filePath;
        }

        protected List<T> GetAllInternal()
        {
            if (!File.Exists(FilePath))
            {
                return new List<T>();
            }

            var json = File.ReadAllText(FilePath, Encoding.UTF8);
            return JsonSerializer.Deserialize<List<T>>(json) ?? new List<T>();
        }
        public List<T> GetAll()
        {
            return GetAllInternal();
        }

        protected void SaveAll(List<T> items)
        {
            var json = JsonSerializer.Serialize(items, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(FilePath, json, Encoding.UTF8);
        }

        public virtual void Add(T item)
        {
            var items = GetAllInternal();
            var nextId = items.Any() ? items.Max(i => i.Id) + 1 : 1;
            item.Id = nextId;
            items.Add(item);
            SaveAll(items);
        }

        public void Delete(int id)
        {
            var items = GetAllInternal();
            var item = items.FirstOrDefault(p => p.Id == id);

            if (item != null)
            {
                items.Remove(item);
                SaveAll(items);
            }
        }

        public T? GetById(int id)
        {
            var items = GetAllInternal();
            return items.FirstOrDefault(i => i.Id == id);
        }
    }
}
