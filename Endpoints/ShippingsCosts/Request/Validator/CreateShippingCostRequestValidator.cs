using FastEndpoints;

using FluentValidation;

namespace reymani_web_api.Endpoints.ShippingCost.Request.Validator;

public class CreateShippingCostRequestValidator : Validator<CreateShippingCostRequest>
{
  public CreateShippingCostRequestValidator()
  {
    RuleFor(e => e.MunicipalityId)
      .NotEmpty()
      .GreaterThan(0);

    RuleFor(e => e.VehicleTypeId)
      .NotEmpty()
      .GreaterThan(0);

    RuleFor(e => e.Cost)
      .NotEmpty()
      .GreaterThan(0);
  }
}
