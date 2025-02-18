using System;

using FastEndpoints;

using FluentValidation;

namespace reymani_web_api.Endpoints.Users.Requests.Validators;

public class SearchUsersRequestValidator : Validator<SearchUsersRequest>
{
  public SearchUsersRequestValidator()
  {
    RuleFor(x => x.Page)
        .GreaterThan(0);

    RuleFor(x => x.PageSize)
        .GreaterThan(0);
  }

}