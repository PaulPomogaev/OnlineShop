using System.ComponentModel.DataAnnotations;

namespace OnlineShopWebApp.Models
{
    public class ChangePassword
    {
        public int UserId { get; set; }

        [Required(ErrorMessage = "Не указан старый пароль")]
        [Display(Name = "Старый пароль")]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "Необходимо указать новый пароль")]
        [Display(Name = "Новый пароль")]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "Размер  пароля должен составлять от 6 до 50 символов")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*()_+]).*$",
            ErrorMessage = "Пароль должен содержать хотя бы одну заглавную букву, одну строчную, символ и цифру")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "Подтверждение пароля обязательно")]
        [Display(Name = "Подтвердите новый пароль")]
        [Compare("NewPassword", ErrorMessage = "Пароли не совпадают")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
