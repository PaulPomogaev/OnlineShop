using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace OnlineShop.Core.Models.Reviews
{
    public class Review
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int UserId { get; set; }
        public DateTime? DeletedAt { get; set; }
        public string? DeletedBy { get; set; }
        public string? DeleteReason { get; set; }
        public string? Text { get; set; }
        public int Grade { get; set; }
        public DateTime CreateDate { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; } = "1";
    }
}
