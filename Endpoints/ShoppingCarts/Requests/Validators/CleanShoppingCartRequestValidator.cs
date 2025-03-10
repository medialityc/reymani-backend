using FastEndpoints;

using FluentValidation;


namespace reymani_web_api.Endpoints.ShoppingCarts.Requests.Validators;

public class CleanShoppingCartRequestValidator : Validator<CleanShoppingCartRequest>
{
  public CleanShoppingCartRequestValidator()
  {
    RuleFor(x => x.Id).NotEmpty().GreaterThan(0);
  }
}
