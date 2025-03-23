using FastEndpoints;

using FluentValidation;

namespace reymani_web_api.Endpoints.ShoppingCartItems.Requests.Validator;

public class GetByIdShoppingCartItemRequestValidator : Validator<GetByIdShoppingCartItemRequest>
{
  public GetByIdShoppingCartItemRequestValidator()
  {
    RuleFor(x => x.Id).NotEmpty().GreaterThan(0);
    RuleFor(x => x.ShoppingCartId).NotEmpty().GreaterThan(0);
  }
}
