using FastEndpoints;

using FluentValidation;

namespace reymani_web_api.Endpoints.Orders.Requests.Validator;

public class ConfirmCompletedOrderRequestValidator : Validator<ConfirmCompletedOrderRequest>
{
  public ConfirmCompletedOrderRequestValidator()
  {
    RuleFor(x => x.Id)
        .GreaterThan(0);
  }
}
