using System;

using FastEndpoints;

using FluentValidation;

namespace reymani_web_api.Endpoints.Auth.Requests.Validators;

public class ForgotPasswordValidator : Validator<ForgotPasswordRequest>
{
  public ForgotPasswordValidator()
  {
    RuleFor(x => x.Email).NotEmpty().EmailAddress();
  }

}
