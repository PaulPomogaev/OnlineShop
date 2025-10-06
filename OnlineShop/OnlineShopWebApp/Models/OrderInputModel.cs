using System.ComponentModel.DataAnnotations;

namespace OnlineShopWebApp.Models
{
    public class OrderInputModel
    {
        [Required(ErrorMessage = "Укажите ваше имя")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Имя должно быть от 2 до 100 символов")]
        public string CustomerName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Укажите адрес доставки")]
        [StringLength(200, MinimumLength = 5, ErrorMessage = "Адрес должен быть от 5 до 200 символов")]
        public string Address { get; set; } = string.Empty;

        [Required(ErrorMessage = "Укажите телефон")]
        [Phone(ErrorMessage = "Некорректный формат телефона")]
        [StringLength(20, MinimumLength = 10, ErrorMessage = "Телефон должен содержать от 10 до 20 цифр")]
        public string Phone { get; set; } = string.Empty;

        [Required(ErrorMessage = "Укажите дату доставки")]
        public DateOnly? DeliveryDate { get; set; }

        [StringLength(500, ErrorMessage = "Комментарий не должен превышать 500 символов")]
        public string? Comment { get; set; }
    }
}
