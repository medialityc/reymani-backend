using FastEndpoints;
using FluentValidation;

namespace reymani_web_api.Endpoints.ShippingCost.Request.Validator;

public class GetShippingCostByIdRequestValidator : Validator<GetShippingCostByIdRequest>
{
  public GetShippingCostByIdRequestValidator()
  {
    RuleFor(e => e.Id)
      .NotEmpty()
      .GreaterThan(0);
  }
}
