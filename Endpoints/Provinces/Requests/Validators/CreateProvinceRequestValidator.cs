using FastEndpoints;

using FluentValidation;

namespace reymani_web_api.Endpoints.Provinces.Requests.Validators;

public class CreateProvinceRequestValidator : Validator<CreateProvinceRequest>
{
  public CreateProvinceRequestValidator()
  {
    RuleFor(x => x.Name).NotEmpty();
  }
}
