using OnlineShop.Core.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace OnlineShop.Db.Models;

public class Product : IBaseId
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required decimal Cost { get; set; }
    public string? Description { get; set; }
    public List<CartItem>? CartItems { get; set; }
    public List<OrderItem>? OrderItems { get; set; }
    public string? PhotoPath { get; set; }
}
