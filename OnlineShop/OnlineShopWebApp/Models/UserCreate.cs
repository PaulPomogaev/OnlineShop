using System.ComponentModel.DataAnnotations;

namespace OnlineShopWebApp.Models
{
    public class UserCreate
    {
        [Required(ErrorMessage = "Логин обязателен")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Размер логина должен составлять от 2 до 50 символов")]
        public string Login { get; set; } = string.Empty;

        [Required(ErrorMessage = "Необходимо указать пароль")]
        [Display(Name = "Новый пароль")]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "Размер  пароля должен составлять от 6 до 50 символов")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*()_+]).*$",
            ErrorMessage = "Пароль должен содержать хотя бы одну заглавную букву, одну строчную, символ и цифру")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Подтверждение пароля обязательно")]
        [Display(Name = "Подтвердите новый пароль")]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; } = string.Empty;

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
        public string Phone { get; set; } = string.Empty;
    }
}
