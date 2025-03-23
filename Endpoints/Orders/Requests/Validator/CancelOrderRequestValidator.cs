using FastEndpoints;

using FluentValidation;



namespace reymani_web_api.Endpoints.Orders.Requests.Validator;

public class CancelOrderRequestValidator : Validator<CancelOrderRequest>
{
  public CancelOrderRequestValidator()
  {
    RuleFor(x => x.Id)
        .GreaterThan(0);
  }
}
