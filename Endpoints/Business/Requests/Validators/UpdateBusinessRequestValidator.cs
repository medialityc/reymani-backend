using FastEndpoints;

using FluentValidation;

using reymani_web_api.Endpoints.Business.Requests;
using reymani_web_api.Utils.Validations;

namespace reymani_web_api.Endpoints.Business.Requests.Validators
{
  public class UpdateBusinessRequestValidator : Validator<UpdateBusinessRequest>
  {
    public UpdateBusinessRequestValidator()
    {
      RuleFor(x => x.Id).GreaterThan(0);

      RuleFor(x => x.Name).NotEmpty();

      RuleFor(x => x.Address).NotEmpty();

      RuleFor(x => x.UserId).NotEmpty().GreaterThan(0);

      RuleFor(x => x.MunicipalityId).NotEmpty().GreaterThan(0);

      RuleFor(x => x.Logo)
          .Must(file => file == null || (ImageValidations.BeAValidImage(file) && ImageValidations.HaveValidLength(file)))
          .WithMessage("Logo must be a valid image and have a valid size.");

      RuleFor(x => x.Banner)
          .Must(file => file == null || (ImageValidations.BeAValidImage(file) && ImageValidations.HaveValidLength(file)))
          .WithMessage("Banner must be a valid image and have a valid size.");

      RuleFor(x => x.IsAvailable).NotNull();

      RuleFor(x => x.IsActive).NotNull();
    }
  }
}
