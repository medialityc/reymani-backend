using FastEndpoints;

using FluentValidation;

namespace reymani_web_api.Endpoints.Provinces.Requests.Validators;

public class DeleteProvinceRequestValidator : Validator<DeleteProvinceRequest>
{
  public DeleteProvinceRequestValidator()
  {
    RuleFor(e => e.Id)
      .NotEmpty()
      .GreaterThan(0);
  }
}
