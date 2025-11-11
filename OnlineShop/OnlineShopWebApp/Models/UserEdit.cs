using System.ComponentModel.DataAnnotations;

namespace OnlineShopWebApp.Models
{
    public class UserEdit
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Указание фамилии обязательно")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Размер  фамилии должен составлять от 1 до 50 символов")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Указание имени обязательно")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Размер  имени должен составлять от 1 до 50 символов")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email обязателен")]
        [EmailAddress(ErrorMessage = "Неверный формат Email")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Телефон обязателен")]
        [Phone(ErrorMessage = "Неверный формат телефона")]
        public string PhoneNumber { get; set; } = string.Empty;
    }
}
