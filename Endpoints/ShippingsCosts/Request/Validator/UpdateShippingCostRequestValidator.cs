using FastEndpoints;
using FluentValidation;

using reymani_web_api.Endpoints.ShippingCost.Request;

namespace reymani_web_api.Endpoints.ShippingsCosts.Request.Validator;

public class UpdateShippingCostRequestValidator : Validator<UpdateShippingCostRequest>
{
  public UpdateShippingCostRequestValidator()
  {
    RuleFor(e => e.Id)
      .NotEmpty()
      .GreaterThan(0);

    RuleFor(e => e.Cost)
      .NotEmpty()
      .GreaterThan(0);
  }
}
