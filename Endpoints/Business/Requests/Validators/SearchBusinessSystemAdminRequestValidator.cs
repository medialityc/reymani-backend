using System;

using FastEndpoints;

using FluentValidation;

namespace reymani_web_api.Endpoints.Business.Requests.Validators;

public class SearchBusinessSystemAdminRequestValidator : Validator<SearchBusinessSystemAdminRequest>
{
  public SearchBusinessSystemAdminRequestValidator()
  {
    RuleFor(x => x.Page)
        .GreaterThan(0);

    RuleFor(x => x.PageSize)
        .GreaterThan(0);
  }

}
