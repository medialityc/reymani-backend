using System;

using FastEndpoints;

using FluentValidation;

using reymani_web_api.Utils.Validations;

namespace reymani_web_api.Endpoints.Business.Requests.Validators;

public class UpdateMyBusinessRequestValidator : Validator<UpdateMyBusinessRequest>
{
  public UpdateMyBusinessRequestValidator()
  {
    RuleFor(x => x.Name).NotEmpty();

    RuleFor(x => x.Address).NotEmpty();

    RuleFor(x => x.MunicipalityId).NotEmpty().GreaterThan(0);

    RuleFor(x => x.Logo)
        .Must(file => file == null || (ImageValidations.BeAValidImage(file) && ImageValidations.HaveValidLength(file)))
        .WithMessage("Logo must be a valid image and have a valid size.");

    RuleFor(x => x.Banner)
        .Must(file => file == null || (ImageValidations.BeAValidImage(file) && ImageValidations.HaveValidLength(file)))
        .WithMessage("Banner must be a valid image and have a valid size.");

    RuleFor(x => x.IsAvailable).NotNull();
  }
}
