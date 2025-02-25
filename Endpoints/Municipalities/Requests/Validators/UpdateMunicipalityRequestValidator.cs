using FastEndpoints;
using FluentValidation;

using reymani_web_api.Endpoints.Provinces.Requests;

namespace reymani_web_api.Endpoints.Municipalities.Requests.Validators;

public class UpdateMunicipalityRequestValidator : Validator<UpdateMunicipalityRequest>
{
  public UpdateMunicipalityRequestValidator()
  {
    RuleFor(x => x.Name).NotEmpty();

    RuleFor(e => e.ProvinceId)
      .NotEmpty()
      .GreaterThan(0);

    RuleFor(e => e.Id)
      .NotEmpty()
      .GreaterThan(0);
  }
}
