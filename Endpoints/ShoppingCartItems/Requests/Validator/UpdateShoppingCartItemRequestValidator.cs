using FastEndpoints;

using FluentValidation;

namespace reymani_web_api.Endpoints.ShoppingCartItems.Requests.Validator;

public class UpdateShoppingCartItemRequestValidator : Validator<UpdateShoppingCartItemRequest>
{
  public UpdateShoppingCartItemRequestValidator()
  {
    RuleFor(x => x.Id).GreaterThan(0);
    RuleFor(x => x.ShoppingCartId).GreaterThan(0);
    RuleFor(x => x.Quantity).GreaterThan(0);
  }
}
