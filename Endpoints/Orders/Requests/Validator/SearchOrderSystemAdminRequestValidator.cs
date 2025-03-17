using FastEndpoints;

using FluentValidation;

namespace reymani_web_api.Endpoints.Orders.Requests.Validator;

public class SearchOrderSystemAdminRequestValidator : Validator<SearchOrderSystemAdminRequest>
{
  public SearchOrderSystemAdminRequestValidator()
  {
    RuleFor(x => x.Page)
        .GreaterThan(0);

    RuleFor(x => x.PageSize)
        .GreaterThan(0);
  }
}
