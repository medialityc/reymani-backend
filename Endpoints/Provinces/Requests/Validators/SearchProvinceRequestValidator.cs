using FastEndpoints;

using FluentValidation;


namespace reymani_web_api.Endpoints.Provinces.Requests.Validators;

public class SearchProvinceRequestValidator : Validator<SearchProvincesRequest>
{
  public SearchProvinceRequestValidator()
  {
    RuleFor(x => x.Page)
        .GreaterThan(0);

    RuleFor(x => x.PageSize)
        .GreaterThan(0);
  }
}
