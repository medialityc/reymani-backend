using FastEndpoints;

using FluentValidation;

namespace reymani_web_api.Endpoints.Orders.Requests.Validator;

public class GetOrderByIdInElaboratedRequestValidator : Validator<GetOrderByIdInElaboratedRequest>
{
  public GetOrderByIdInElaboratedRequestValidator()
  {
    RuleFor(x => x.Id)
        .GreaterThan(0);
  }
}
