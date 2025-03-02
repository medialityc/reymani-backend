using FastEndpoints;

using FluentValidation;

namespace reymani_web_api.Endpoints.VehiclesTypes.Requests.Validators;

public class SearchVehicleTypeCourierRequestValidator : Validator<SearchVehicleTypeCourierRequest>
{
  public SearchVehicleTypeCourierRequestValidator()
  {
    RuleFor(x => x.Page)
    .GreaterThan(0);

    RuleFor(x => x.PageSize)
    .GreaterThan(0);
  }
}
