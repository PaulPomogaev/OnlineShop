using OnlineShop.Core.Models;
using OnlineShop.Db.Models;
using OnlineShopWebApp.Models;

namespace OnlineShopWebApp.Helpers
{
    public static class Mapper
    {
        public static ProductViewModel ToViewModel(this Product product)
        {
            return new ProductViewModel
            {
                Id = product.Id,
                Name = product.Name,
                Cost = product.Cost,
                Description = product.Description,
                PhotoPath = !string.IsNullOrEmpty(product.PhotoPath) ? product.PhotoPath : "/img/whey-protein.jpg",
                ImagePaths = product.ImagePaths ?? new List<string>()
            };
        }

        public static List<ProductViewModel> ToViewModels(this List<Product> products)
        {
            return products.Select(p => p.ToViewModel()).ToList();
        }

        public static OrderViewModel ToViewModel(this Order order)
        {
            return new OrderViewModel
            {
                Id = order.Id,
                UserId = order.UserId,
                CreatedDate = order.CreatedDate,
                DeliveryDate = order.DeliveryDate,
                Comment = order.Comment,
                Status = order.Status,
                Customer = order.Customer,
                Items = order.Items.Where(item => item.Product != null).Select(item => new OrderItemViewModel
                {
                    Id = item.Id,
                    Quantity = item.Quantity,
                    Product = item.Product.ToViewModel()
                }).ToList(),
                InputModel = new OrderInputModel()
            };
        }

        public static List<OrderViewModel> ToViewModels (this List<Order> orders)
        {

            return orders.Select(o => o.ToViewModel()).ToList();
        }


        public static CartItemViewModel ToViewModel(this CartItem cartItem)
        {
            if (cartItem.Product == null)
            {
                throw new InvalidOperationException("Позиция в корзине не содержит продуктов.");
            }
                

            return new CartItemViewModel
            {
                Id = cartItem.Id,
                Quantity = cartItem.Quantity,
                Product = cartItem.Product.ToViewModel() 
            };
        }

        public static CartViewModel ToViewModel(this Cart cart)
        {
            return new CartViewModel
            {
                Id = cart.Id,
                UserId = cart.UserId,
                Items = cart.Items?.Select(item => item.ToViewModel()).ToList() ?? new List<CartItemViewModel>()
            };
        }
    }
}