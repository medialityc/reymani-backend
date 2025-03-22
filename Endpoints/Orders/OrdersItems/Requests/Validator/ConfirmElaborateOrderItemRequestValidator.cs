using FastEndpoints;

using FluentValidation;

using reymani_web_api.Endpoints.Orders.OrdersItems.Requests;

namespace reymani_web_api.Endpoints.Orders.OrdersItems.Requests.Validator;

public class ConfirmElaborateOrderItemRequestValidator : Validator<ConfirmElaborateOrderItemRequest>
{
  public ConfirmElaborateOrderItemRequestValidator()
  {
    RuleFor(x => x.OrderId)
        .GreaterThan(0);
    RuleFor(x => x.OrderItemId)
       .GreaterThan(0);
  }
}
