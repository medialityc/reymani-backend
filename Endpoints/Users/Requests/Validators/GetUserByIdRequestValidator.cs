using System;

using FastEndpoints;

using FluentValidation;

namespace reymani_web_api.Endpoints.Users.Requests.Validators;

public class GetUserByIdRequestValidator : Validator<GetUserByIdRequest>
{
  public GetUserByIdRequestValidator()
  {
    RuleFor(e => e.Id)
      .NotEmpty()
      .GreaterThan(0);
  }
}