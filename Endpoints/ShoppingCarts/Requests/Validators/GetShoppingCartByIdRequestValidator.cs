using FastEndpoints;

using FluentValidation;

namespace reymani_web_api.Endpoints.ShoppingCarts.Requests.Validators;

public class GetShoppingCartByIdRequestValidator : Validator<GetShoppingCartByIdRequest>
{
  public GetShoppingCartByIdRequestValidator()
  {
    RuleFor(x => x.ShoppingCartId).NotEmpty().GreaterThan(0);
  }
}
