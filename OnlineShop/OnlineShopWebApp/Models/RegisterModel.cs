using System.ComponentModel.DataAnnotations;

namespace OnlineShopWebApp.Models
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "Не указан логин")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "Длина должна быть от 2 до 30 символов")]
        public string Login { get; set; }
        [Required(ErrorMessage = "Не указан пароль")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Пароль должен быть от 6 до 100 символов")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*()_+]).*$",
        ErrorMessage = "Пароль должен содержать хотя бы одну заглавную букву, одну строчную, символ и цифру")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Не указан повторный пароль")]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        public string ConfirmPassword { get; set; }
    }
}
