using OnlineShop.Core.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace OnlineShopWebApp.Models
{
    public class RoleViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Необходимо ввести наименование роли")]
        [Display(Name = "Наименование роли", Prompt = "Введите наименование роли")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Название роли должно быть от 2 до 50 символов")]
        public string Name { get; set; }
    }
}
