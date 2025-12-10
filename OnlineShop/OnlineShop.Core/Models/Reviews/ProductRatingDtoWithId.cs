using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Core.Models.Reviews
{
    public class ProductRatingDtoWithId
    {
        public int ProductId { get; set; }
        public double Rating { get; set; }
        public int ReviewCount { get; set; }
    }
}
