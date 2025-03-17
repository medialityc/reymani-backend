using FastEndpoints;
using FluentValidation;


namespace reymani_web_api.Endpoints.OrdersItems.Requests.Validator;

public class ConfirmPickUpOrderItemRequestValidator : Validator<ConfirmPickUpOrderItemRequest>
{
  public ConfirmPickUpOrderItemRequestValidator()
  {
    RuleFor(x => x.OrderId)
        .GreaterThan(0);
    RuleFor(x => x.OrderItemId)
       .GreaterThan(0);
  }
}
