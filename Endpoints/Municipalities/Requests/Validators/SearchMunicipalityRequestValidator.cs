using FastEndpoints;

using FluentValidation;

namespace reymani_web_api.Endpoints.Municipalities.Requests.Validators;

public class SearchMunicipalityRequestValidator : Validator<SearchMunicipalityRequest>
{
  public SearchMunicipalityRequestValidator()
  {
    RuleFor(x => x.Page)
        .GreaterThan(0);

    RuleFor(x => x.PageSize)
        .GreaterThan(0);
  }
}
