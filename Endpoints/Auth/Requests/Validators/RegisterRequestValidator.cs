using System;

using FastEndpoints;

using FluentValidation;

using reymani_web_api.Utils.Validations;

namespace reymani_web_api.Endpoints.Auth.Requests.Validators;

public class RegisterRequestValidator : Validator<RegisterRequest>
{
  public RegisterRequestValidator()
  {
    RuleFor(x => x.Email).NotEmpty().EmailAddress();

    RuleFor(x => x.Password)
      .NotEmpty()
      .MinimumLength(8)
      .Matches(@"[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
      .Matches(@"[0-9]").WithMessage("Password must contain at least one number.")
      .Matches(@"[\W]").WithMessage("Password must contain at least one special character."); ;

    RuleFor(x => x.FirstName).NotEmpty();

    RuleFor(x => x.LastName).NotEmpty();

    RuleFor(x => x.Phone).NotEmpty();

    RuleFor(x => x.ProfilePicture).Must(ImageValidations.BeAValidImage);
  }
}
