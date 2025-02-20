using FastEndpoints;

using FluentValidation;

namespace reymani_web_api.Endpoints.Municipalities.Requests.Validators;

public class DeleteMunicipalityRequestValidator : Validator<DeleteMunicipalityRequest>
{
  public DeleteMunicipalityRequestValidator()
  {
    RuleFor(e => e.Id)
      .NotEmpty()
      .GreaterThan(0);
  }
}
