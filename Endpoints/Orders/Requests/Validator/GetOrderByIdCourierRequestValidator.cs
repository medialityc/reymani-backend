using FastEndpoints;

using FluentValidation;

namespace reymani_web_api.Endpoints.Orders.Requests.Validator;

public class GetOrderByIdCourierRequestValidator : Validator<GetOrderByIdCourierRequest>
{
  public GetOrderByIdCourierRequestValidator()
  {
    RuleFor(x => x.Id)
        .GreaterThan(0);
  }
}
