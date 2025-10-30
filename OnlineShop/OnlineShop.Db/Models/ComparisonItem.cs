using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Db.Models
{
    public class ComparisonItem
    {
        public int Id { get; set; }
        public int ComparisonId { get; set; }
        public int ProductId { get; set; }
        public Comparison Comparison { get; set; }
        public Product Product { get; set; }
    }
}
