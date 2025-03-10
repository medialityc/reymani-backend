using FastEndpoints;

using FluentValidation;


namespace reymani_web_api.Endpoints.ShoppingCartItems.Requests.Validator;

public class CreateShoppingCartItemRequestValidator : Validator<CreateShoppingCartItemRequest>
{
  public CreateShoppingCartItemRequestValidator()
  {
    RuleFor(x => x.Quantity).NotEmpty().GreaterThan(0);
    RuleFor(x => x.ProductId).NotEmpty().GreaterThan(0);
    RuleFor(x => x.ShoppingCartId).NotEmpty().GreaterThan(0);
  }
}
