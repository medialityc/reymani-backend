using FastEndpoints;

using FluentValidation;

namespace reymani_web_api.Endpoints.VehiclesTypes.Requests.Validators;

public class GetByIdRequestValidator : Validator<GetByIdRequest>
{
  public GetByIdRequestValidator()
  {
    RuleFor(e => e.Id)
      .NotEmpty()
      .GreaterThan(0);
  }
}
