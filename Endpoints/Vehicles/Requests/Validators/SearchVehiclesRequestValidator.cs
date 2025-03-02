using FastEndpoints;

using FluentValidation;

namespace reymani_web_api.Endpoints.Vehicles.Requests.Validators;

public class SearchVehiclesRequestValidator: Validator<SearchVehiclesRequest>
{
  public SearchVehiclesRequestValidator()
  {
    RuleFor(x => x.Page)
    .GreaterThan(0);

    RuleFor(x => x.PageSize)
        .GreaterThan(0);
  }
}
