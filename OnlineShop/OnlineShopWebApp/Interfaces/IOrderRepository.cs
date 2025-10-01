using OnlineShopWebApp.Models;

namespace OnlineShopWebApp.Interfaces
{
    public interface IOrderRepository
    {
        List<Order> GetAllOrders();
        void AddOrder(Order order);
        Order? GetOrderById(int id);
        Order CreateOrder(Cart cart);
        Order CreateOrder(Cart cart, OrderInputModel input);
    }
}
