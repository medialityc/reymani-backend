using FastEndpoints;

using FluentValidation;

using reymani_web_api.Endpoints.Vehicles.Requests;

namespace reymani_web_api.Endpoints.VehiclesTypes.Requests.Validators;

public class SearchVehicleTypeAdminRequestValidator : Validator<SearchVehicleTypeAdminRequest>
{
  public SearchVehicleTypeAdminRequestValidator()
  {
    RuleFor(x => x.Page)
    .GreaterThan(0);

    RuleFor(x => x.PageSize)
    .GreaterThan(0);
  }
}
