using OnlineShopWebApp.Models;

namespace OnlineShopWebApp.Interfaces
{
    public interface IOrderRepository
    {
        List<Order> GetAllOrders();
        void AddOrder(Order order);
        Order? GetOrderById(int id);
        Order CreateOrderFromCart(Cart cart, string customerName, string address, string phone);
    }
}
