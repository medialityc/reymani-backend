using System;

using FastEndpoints;

using FluentValidation;

namespace reymani_web_api.Endpoints.Products.Requests.Validators;

public class SearchProductsRequestValidator : Validator<SearchProductsRequest>
{
  public SearchProductsRequestValidator()
  {
    RuleFor(x => x.Page)
        .GreaterThan(0);

    RuleFor(x => x.PageSize)
        .GreaterThan(0);
  }
}