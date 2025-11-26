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
                Items = order.Items.Select(item => new OrderItemViewModel
                {
                    Id = item.Id,
                    Quantity = item.Quantity,
                    Price = item.Price,
                    Product = item.Product?.ToViewModel() ?? new ProductViewModel
                    {
                        Id = 0, 
                        Name = "Товар не найден",
                        Cost = 0,
                        Description = string.Empty, 
                        PhotoPath = "/img/whey-protein.jpg", 
                        ImagePaths = new List<string>() 
                    }
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

        public static UserEdit ToEditModel(this User user)
        {
            return new UserEdit
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                AvatarPath = user.AvatarPath 
            };
        }

        public static void UpdateFromEditModel(this User user, UserEdit model)
        {
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Email = model.Email;
            user.PhoneNumber = model.PhoneNumber;
            user.UserName = model.Email; 
        }
    }
}