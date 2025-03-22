using FastEndpoints;

using FluentValidation;

namespace reymani_web_api.Endpoints.Orders.Requests.Validator;

public class ConfirmDeliveredOrderRequestValidator : Validator<ConfirmDeliveredOrderRequest>
{
  public ConfirmDeliveredOrderRequestValidator()
  {
    RuleFor(x => x.Id)
        .GreaterThan(0);
  }
}
