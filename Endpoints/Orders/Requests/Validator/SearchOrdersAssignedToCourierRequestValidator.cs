using FastEndpoints;

using FluentValidation;



namespace reymani_web_api.Endpoints.Orders.Requests.Validator;

public class SearchOrdersAssignedToCourierRequestValidator : Validator<SearchOrdersAssignedToCourierRequest>
{
  public SearchOrdersAssignedToCourierRequestValidator()
  {
    RuleFor(x => x.Page)
        .GreaterThan(0);

    RuleFor(x => x.PageSize)
        .GreaterThan(0);
  }
}
