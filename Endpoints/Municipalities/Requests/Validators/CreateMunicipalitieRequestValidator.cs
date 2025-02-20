using FastEndpoints;

using FluentValidation;

namespace reymani_web_api.Endpoints.Municipalities.Requests.Validators;

public class CreateMunicipalitieRequestValidator : Validator<CreateMunicipalityRequest>
{
  public CreateMunicipalitieRequestValidator()
  {
    RuleFor(x => x.Name).NotEmpty();
    RuleFor(x => x.ProvinceId)
        .NotEmpty()
        .GreaterThan(0);
  }

}
