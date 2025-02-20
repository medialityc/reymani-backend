using FastEndpoints;

using FluentValidation;

namespace reymani_web_api.Endpoints.Municipalities.Requests.Validators;

public class GetMunicipalitieByIdRequestValidator : Validator<GetMunicipalityByIdRequest>
{
  public GetMunicipalitieByIdRequestValidator() 
  {
    RuleFor(e => e.Id)
      .NotEmpty()
      .GreaterThan(0);
  }
}
