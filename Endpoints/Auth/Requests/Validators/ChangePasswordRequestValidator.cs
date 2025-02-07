using System;

using FastEndpoints;

using FluentValidation;

namespace reymani_web_api.Endpoints.Auth.Requests.Validators;

public class ChangePasswordRequestValidator : Validator<ChangePasswordRequest>
{
  public ChangePasswordRequestValidator()
  {
    RuleFor(x => x.CurrentPassword).NotEmpty();
    RuleFor(x => x.NewPassword)
      .NotEmpty()
      .MinimumLength(8)
      .Matches(@"[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
      .Matches(@"[0-9]").WithMessage("Password must contain at least one number.")
      .Matches(@"[\W]").WithMessage("Password must contain at least one special character.");

    RuleFor(x => x)
      .Must(x => x.CurrentPassword != x.NewPassword)
      .WithMessage("New password must be different from the current password.");
  }
}
