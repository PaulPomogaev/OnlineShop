using OnlineShopWebApp.Models;
using System.Text.Json;
using System.Text;
using OnlineShopWebApp.Interfaces;

namespace OnlineShopWebApp.Data
{
    public class OrderJsonRepository : IOrderRepository
    {
        private readonly string _filePath = "Data/orders.json";
        private int _nextId;
        private int _nextItemId;

        public OrderJsonRepository()
        {
            InitializeNextIds();
        }

        private void InitializeNextIds()
        {
            var orders = GetAllOrders();
            _nextId = orders.Any() ? orders.Max(o => o.Id) + 1 : 1;

            var allItemIds = orders.SelectMany(o => o.Items.Select(i => i.Id)).ToList();
            _nextItemId = allItemIds.Any() ? allItemIds.Max() + 1 : 1;
        }

        public List<Order> GetAllOrders()
        {
            if (!File.Exists(_filePath))
            {
                return new List<Order>();
            }

            var json = File.ReadAllText(_filePath, Encoding.UTF8);
            return JsonSerializer.Deserialize<List<Order>>(json) ?? new List<Order>();
        }

        public void AddOrder(Order order)
        {
            order.Id = _nextId++;

            foreach (var item in order.Items)
            {
                item.Id = _nextItemId++;
            }

            var orders = GetAllOrders();
            orders.Add(order);
            SaveAllOrders(orders);
        }

        public Order? GetOrderById(int id)
        {
            var orders = GetAllOrders();
            return orders.FirstOrDefault(o => o.Id == id);
        }

        private void SaveAllOrders(List<Order> orders)
        {
            var json = JsonSerializer.Serialize(orders, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_filePath, json, Encoding.UTF8);
        }

        public Order CreateOrder(Cart cart)
        {
            return CreateOrder(cart, string.Empty, string.Empty, string.Empty);
        }

        public Order CreateOrder (Cart cart, string customerName, string address, string phone)
        {
            var orderItems = cart.Items.Select(item => new OrderItem
            {
                Product = item.Product,
                Quantity = item.Quantity
            }).ToList();

            return new Order
            {
                UserId = cart.UserId,
                Items = orderItems,
                CustomerName = customerName,
                Address = address,
                Phone = phone,
                CreatedDate = DateTime.Now

            };
        }

    }
}
