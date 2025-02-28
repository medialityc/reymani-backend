using FastEndpoints;

using FluentValidation;

namespace reymani_web_api.Endpoints.ShippingCost.Request.Validator;

public class DeleteShippingCostRequestValidator : Validator<DeleteShippingCostRequest>
{
  public DeleteShippingCostRequestValidator() 
  {
    RuleFor(e => e.Id)
      .NotEmpty()
      .GreaterThan(0);
  }
}
