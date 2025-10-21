using OnlineShopWebApp.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace OnlineShopWebApp.Models
{
    public class Product : IBaseId
    {
        public string PhotoPath { get; set; } = "img/whey-protein.jpg";
        public required int Id { get; set; }

        [Required(ErrorMessage = "Поле наименования товара обязательно к заполнению")]
        [StringLength(200, MinimumLength = 2, ErrorMessage = "Длина наименования товара должна быть от 2 до 200 символов")]
        [Display(Name = "Наименования товара", Prompt = "Введите название товара")]
        public required string Name { get; set; }

        [Required(ErrorMessage = "Поле цены товара обязательно к заполнению")]
        [Range(0, 1000000, ErrorMessage = "Цена должна быть в диапазоне от 0 до 1 000 000 руб.")]
        [Display(Name = "Цена, руб.", Prompt = "Введите цену товара")]
        public required decimal Cost { get; set; }

        [StringLength(4096, ErrorMessage = "Длина описания товара не должна быть более 4096 символов")]
        [Display(Name = "Описание товара", Prompt = "Введите описание товара")]
        public  string? Description { get; set; }
               
    }
}
