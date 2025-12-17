using FluentValidation;
using OnlineShop.Core.Models.Products.Commands;

namespace OnlineShop.Core.Validators
{
    public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Название товара обязательно для заполнения")
                .MaximumLength(100).WithMessage("Название товара не должно превышать 100 символов")
                .MinimumLength(3).WithMessage("Название товара должно содержать минимум 3 символа");

            RuleFor(x => x.Cost)
                .GreaterThan(0).WithMessage("Стоимость товара должна быть больше нуля")
                .LessThanOrEqualTo(1000000).WithMessage("Стоимость товара не может превышать 1 000 000");

            RuleFor(x => x.Description)
                .MaximumLength(2000).WithMessage("Описание товара не должно превышать 2000 символов")
                .When(x => !string.IsNullOrEmpty(x.Description));

            RuleFor(x => x.PhotoPath)
                .MaximumLength(500).WithMessage("Путь к изображению слишком длинный")
                .When(x => !string.IsNullOrEmpty(x.PhotoPath));

            RuleForEach(x => x.ImagePaths)
                .MaximumLength(500).WithMessage("Путь к изображению слишком длинный")
                .When(x => x.ImagePaths != null);
        }
    }
}
