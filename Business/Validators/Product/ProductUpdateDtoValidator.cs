using Business.Dtos.Product;
using FluentValidation;

namespace Business.Validators.Product
{
    public class ProductUpdateDtoValidator : AbstractValidator<ProductUpdateDto>
    {
        public ProductUpdateDtoValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required");
            RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price can't be zero");
            RuleFor(x => x.Quantity).GreaterThan(0).WithMessage("Quantity must be at least 1");
            RuleFor(x => x.Description).MinimumLength(15).MaximumLength(400).WithMessage("Description must contain 15-400 character");
            RuleFor(x => x.Type).IsInEnum().WithMessage("Type is incorrect");
            RuleFor(x => x.Photo).Must(IsCorrectFormat).When(x => x.Photo is not null).WithMessage("Photo must be in format of image");
        }
        private bool IsCorrectFormat(string photo)
        {
            try
            {
                _ = Convert.FromBase64String(photo);
                var data = photo.Substring(0, 5);
                switch (data.ToUpper())
                {
                    case "IVBOR":
                    case "/9J/4":
                        return true;
                    default:
                        return false;
                };
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
