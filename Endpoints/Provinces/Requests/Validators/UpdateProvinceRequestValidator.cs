using FastEndpoints;

using FluentValidation;

namespace reymani_web_api.Endpoints.Provinces.Requests.Validators;

public class UpdateProvinceRequestValidator : Validator<UpdateProvinceRequest>
{
  public UpdateProvinceRequestValidator()
  {

    RuleFor(x => x.Name).NotEmpty();
  }
}
