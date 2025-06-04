using FastEndpoints;

using FluentValidation;

namespace reymani_web_api.Endpoints.Orders.Requests.Validator;

public class CreateOrderRequestValidator : Validator<CreateOrderRequest>
{
  public CreateOrderRequestValidator()
  {
    RuleFor(x => x.CustomerId)
        .GreaterThan(0)
        .NotEmpty();

    RuleFor(x => x.CustomerAddressId)
        .GreaterThan(0)
        .NotEmpty();

    RuleFor(x => x.ShoppingCartId)
        .GreaterThan(0)
        .NotEmpty();

  }
}
