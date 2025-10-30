using OnlineShop.Db.Models;
using OnlineShopWebApp.Models;

namespace OnlineShopWebApp.Helpers
{
    public static class ProductMapper
    {
        public static ProductViewModel ToViewModel(this Product product)
        {
            return new ProductViewModel
            {
                Id = product.Id,
                Name = product.Name,
                Cost = product.Cost,
                Description = product.Description,
                PhotoPath = "/images/products/whey-protein.jpg"
            };
        }

        public static List<ProductViewModel> ToViewModels(this List<Product> products)
        {
            return products.Select(p => p.ToViewModel()).ToList();
        }
    }
}
