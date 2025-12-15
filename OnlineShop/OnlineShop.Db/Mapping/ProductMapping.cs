using OnlineShop.Core.Models.Products;
using OnlineShop.Db.Models;

namespace OnlineShop.Db.Mapping
{
    public static class ProductMapping
    {
        public static ProductDto ToDto (this Product product)
        {
            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Cost = product.Cost,
                Description = product.Description,
                PhotoPath = product.PhotoPath,
                ImagePaths = product.ImagePaths
            };
        }

        public static Product ToEntity( this ProductDto dto)
        {
            return new Product
            {
                Id = dto.Id,
                Name = dto.Name,
                Cost = dto.Cost,
                Description = dto.Description,
                PhotoPath = dto.PhotoPath,
                ImagePaths = dto.ImagePaths
            };
        }

        public static List<ProductDto> ToDtoList(this IEnumerable<Product> products)
        {
            return products.Select(p => p.ToDto()).ToList();
        }
    }
}
