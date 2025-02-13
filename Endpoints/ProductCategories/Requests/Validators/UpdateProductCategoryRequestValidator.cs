using FastEndpoints;

using FluentValidation;

using reymani_web_api.Utils.Validations;

namespace reymani_web_api.Endpoints.ProductCategories.Requests.Validators
{
  public class UpdateProductCategoryRequestValidator : Validator<UpdateProductCategoryRequest>
  {
    public UpdateProductCategoryRequestValidator()
    {
      RuleFor(x => x.Id)
        .GreaterThan(0);

      RuleFor(x => x.Name).NotEmpty();

      RuleFor(x => x.IsActive)
        .NotNull();

      RuleFor(x => x.Logo)
        .Must(file => file == null || (ImageValidations.BeAValidImage(file) && ImageValidations.HaveValidLength(file)));
    }
  }
}
