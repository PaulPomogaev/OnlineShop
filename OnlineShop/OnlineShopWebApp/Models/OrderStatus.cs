using System.ComponentModel.DataAnnotations;

namespace OnlineShopWebApp.Models
{
    public enum OrderStatus
    {
        [Display(Name = "Создан")]
        Created,

        [Display(Name = "Обработан")]
        Processed,

        [Display(Name = "В пути")]
        InTransit,

        [Display(Name = "Доставлен")]
        Delivered,

        [Display(Name = "Отменён")]
        Cancelled,
    }
}
