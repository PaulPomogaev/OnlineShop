using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Core.Models.Reviews
{
    public class AddReviewRequest
    {
        public int ProductId { get; set; }

        public int UserId { get; set; }

        [Required]
        [StringLength(1000, MinimumLength = 1, ErrorMessage = "Текст должен составлять 1–1000 знаков.")]
        public string Text { get; set; } = string.Empty;

        [Range(0, 5, ErrorMessage = "Оценка должна быть от 0 до 5.")]
        public int Grade { get; set; }
    }
}
