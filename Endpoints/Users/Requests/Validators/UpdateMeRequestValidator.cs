using System;

using FastEndpoints;

using FluentValidation;

using reymani_web_api.Utils.Validations;

namespace reymani_web_api.Endpoints.Users.Requests.Validators;

public class UpdateMeRequestValidator : Validator<UpdateMeRequest>
{
  public UpdateMeRequestValidator()
  {
    RuleFor(x => x.Email).NotEmpty().EmailAddress();

    RuleFor(x => x.FirstName).NotEmpty();

    RuleFor(x => x.LastName).NotEmpty();

    RuleFor(x => x.Phone).NotEmpty();

    RuleFor(x => x.ProfilePicture)
          .Must(file => file == null || (ImageValidations.BeAValidImage(file) && ImageValidations.HaveValidLength(file)));
  }
}
