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
            var orders = GetAll();
            _nextId = orders.Any() ? orders.Max(o => o.Id) + 1 : 1;

            var allItemIds = orders.SelectMany(o => o.Items.Select(i => i.Id)).ToList();
            _nextItemId = allItemIds.Any() ? allItemIds.Max() + 1 : 1;
        }

        public List<Order> GetAll()
        {
            if (!File.Exists(_filePath))
            {
                return new List<Order>();
            }

            var json = File.ReadAllText(_filePath, Encoding.UTF8);
            return JsonSerializer.Deserialize<List<Order>>(json) ?? new List<Order>();
        }

        public void Add(Order order)
        {
            order.Id = _nextId++;

            foreach (var item in order.Items)
            {
                item.Id = _nextItemId++;
            }

            var orders = GetAll();
            orders.Add(order);
            SaveAll(orders);
        }

        public Order? GetById(int id)
        {
            var orders = GetAll();
            return orders.FirstOrDefault(o => o.Id == id);
        }

        private void SaveAll(List<Order> orders)
        {
            var json = JsonSerializer.Serialize(orders, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_filePath, json, Encoding.UTF8);
        }

        public Order Create(Cart cart)
        {
            var input = new OrderInputModel
            {
                CustomerName = string.Empty,
                Address = string.Empty,
                Phone = string.Empty,
                DeliveryDate = null,
                Comment = string.Empty
            };
            return Create(cart, input);
        }

        public Order Create (Cart cart, OrderInputModel input)
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
                Customer = new CustomerInfo
                {
                    Name = input.CustomerName,
                    Address = input.Address,
                    Phone = input.Phone
                },
                DeliveryDate = input.DeliveryDate,
                Comment = input.Comment,
                CreatedDate = DateTime.Now

            };
        }

        public void Update(Order updateOrder)
        {
            var orders = GetAll();
            var existingOrder = orders.FirstOrDefault(o => o.Id == updateOrder.Id);

            if(existingOrder != null)
            {
                existingOrder.Status = updateOrder.Status;
                SaveAll(orders);
            }
        }
    }
}
