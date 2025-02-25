using FastEndpoints;

using FluentValidation;

using reymani_web_api.Endpoints.Provinces.Requests;

namespace reymani_web_api.Endpoints.AddressUser.Requests.Validator;

public class SearchCustomerRequestValidator : Validator<SearchCustomerRequest>
{
  public SearchCustomerRequestValidator()
  {
    RuleFor(x => x.Page)
        .GreaterThan(0);

    RuleFor(x => x.PageSize)
        .GreaterThan(0);
  }
}
