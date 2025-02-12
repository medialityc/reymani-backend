using System;

using FastEndpoints;

using FluentValidation;

namespace reymani_web_api.Endpoints.ProductCategories.Requests.Validators;

public class SearchProductCategoriesRequestValidator : Validator<SearchProductCategoriesRequest>
{
  public SearchProductCategoriesRequestValidator()
  {
    RuleFor(x => x.Page)
        .GreaterThan(0);

    RuleFor(x => x.PageSize)
        .GreaterThan(0);
  }
}