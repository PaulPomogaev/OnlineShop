using OnlineShopWebApp.Interfaces;
using OnlineShopWebApp.Models;
using System.Text.Json;
using System.Text;

namespace OnlineShopWebApp.Data
{
    public class RolesJsonRepository : IRolesRepository
    {
        private readonly string _path = "Data/roles.json";

        private int _nextId;

        public RolesJsonRepository()
        {
            InitializeNextIds();
        }

        public void InitializeNextIds()
        {
            var roles = GetAll();
            _nextId = roles.Any() ? roles.Max(r => r.Id) + 1 : 1;
        }

        public List<Role> GetAll()
        {
            if(!File.Exists(_path))
            {
                return new List<Role>();
            }

            var json = File.ReadAllText(_path, Encoding.UTF8);
            return JsonSerializer.Deserialize<List<Role>>(json) ?? new List<Role>();
        }

        public void Add(Role role)
        {
            role.Id = _nextId++;
            var roles = GetAll();
            roles.Add(role);
            SaveAll(roles);
        }

        private void SaveAll (List<Role> roles)
        {
            var json = JsonSerializer.Serialize(roles, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_path, json, Encoding.UTF8);
        }

        public void Delete(int id)
        {
            var roles = GetAll();
            var role = roles.FirstOrDefault(r => r.Id == id);

            if(role != null)
            {
                roles.Remove(role);
                SaveAll(roles);
            }
        }

        public bool Exist(string roleName)
        {
            var roles = GetAll();
            return roles.Any(r => r.Name.Equals(roleName, StringComparison.OrdinalIgnoreCase));
        }

        public Role? GetById(int id)
        {
            var roles = GetAll();
            return roles.FirstOrDefault(r => r.Id == id);
        }

    }
}
