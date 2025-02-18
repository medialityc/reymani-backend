using System;

using FastEndpoints;

using FluentValidation;

using reymani_web_api.Endpoints.Auth.Requests;

namespace reymani_web_api.Endpoints.Auth.Requests.Validators;

public class LoginRequestValidator : Validator<LoginRequest>
{
  public LoginRequestValidator()
  {
    RuleFor(x => x.Email).NotEmpty().EmailAddress();
    RuleFor(x => x.Password).NotEmpty();
  }
}