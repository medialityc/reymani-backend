using FastEndpoints;
using FluentValidation;

using reymani_web_api.Endpoints.Users.Requests;

namespace reymani_web_api.Endpoints.Provinces.Requests.Validators;

public class GetProvinceByIdRequestValidator : Validator<GetProvinceByIdRequest>
{
  public GetProvinceByIdRequestValidator()
  {
    RuleFor(e => e.Id)
      .NotEmpty()
      .GreaterThan(0);
  }
}
