using FastEndpoints;

using FluentValidation;

namespace reymani_web_api.Endpoints.ProductCategories.Requests.Validators;

public class GetProductCategoryByIdRequestValidator : Validator<GetProductCategoryByIdRequest>
{
  public GetProductCategoryByIdRequestValidator()
  {
    RuleFor(x => x.Id)
        .NotEmpty()
        .GreaterThan(0);
  }
}
