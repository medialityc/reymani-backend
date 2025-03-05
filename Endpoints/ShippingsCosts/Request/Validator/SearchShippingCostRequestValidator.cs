using FastEndpoints;

using FluentValidation;

using reymani_web_api.Endpoints.Provinces.Requests;

namespace reymani_web_api.Endpoints.ShippingCost.Request.Validator;

public class SearchShippingCostRequestValidator : Validator<SearchShippingCostRequest>
{
  public SearchShippingCostRequestValidator()
  {
    RuleFor(x => x.Page)
        .GreaterThan(0);

    RuleFor(x => x.PageSize)
        .GreaterThan(0);

    RuleFor(e => e.CostMin)
      .GreaterThan(0);

    RuleFor(e => e.CostMax)
      .GreaterThan(0);
  }
}
