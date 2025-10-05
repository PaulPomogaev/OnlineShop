using System.ComponentModel.DataAnnotations;

namespace OnlineShopWebApp.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Не указан логин")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "Длина должна быть от 2 до 30 символов")]
        public string Login { get; set; }
        [Required(ErrorMessage = "Не указан пароль")]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}
