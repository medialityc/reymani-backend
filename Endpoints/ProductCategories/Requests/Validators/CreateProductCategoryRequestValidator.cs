using FastEndpoints;

using FluentValidation;

using reymani_web_api.Utils.Validations;

namespace reymani_web_api.Endpoints.ProductCategories.Requests.Validators
{
  public class CreateProductCategoryRequestValidator : Validator<CreateProductCategoryRequest>
  {
    public CreateProductCategoryRequestValidator()
    {
      RuleFor(x => x.Name).NotEmpty();

      RuleFor(x => x.Logo)
        .Must(file => file == null || (ImageValidations.BeAValidImage(file) && ImageValidations.HaveValidLength(file)));
    }
  }
}