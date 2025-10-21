using OnlineShopWebApp.Models;
using System.Text.Json;
using System.Text;
using OnlineShopWebApp.Interfaces;

namespace OnlineShopWebApp.Data
{
    public class OrderJsonRepository : BaseJsonRepository<Order>, IOrderRepository
    {
        private int _nextItemId;

        public OrderJsonRepository() : base("Data/orders.json") 
        {
            var orders = GetAllInternal();
            var allItemIds = orders.SelectMany(o => o.Items.Select(i => i.Id)).ToList();
            _nextItemId = allItemIds.Any() ? allItemIds.Max() + 1 : 1;
        }
               
        public override void Add(Order order)
        {
            var orders = GetAllInternal();
            order.Id = orders.Any() ? orders.Max(o => o.Id) + 1 : 1;

            foreach (var item in order.Items)
            {
                item.Id = _nextItemId++;
            }

            orders.Add(order);
            SaveAll(orders);
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

        public void Edit(Order updateOrder)
        {
            var orders = GetAllInternal();
            var existingOrder = orders.FirstOrDefault(o => o.Id == updateOrder.Id);

            if(existingOrder != null)
            {
                existingOrder.Status = updateOrder.Status;
                SaveAll(orders);
            }
        }
    }
}
