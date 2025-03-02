using FastEndpoints;

using FluentValidation;

using reymani_web_api.Utils.Validations;

namespace reymani_web_api.Endpoints.Vehicles.Requests.Validators;

public class UpdateCourierAdminRequestValidator : Validator<UpdateCourierAdminRequest>
{
  public UpdateCourierAdminRequestValidator()
  {
    RuleFor(e => e.Id)
      .NotEmpty()
      .GreaterThan(0);

    RuleFor(x => x.Picture)
       .Must(file => (ImageValidations.BeAValidImage(file) && ImageValidations.HaveValidLength(file)))
       .WithMessage("La imagen debe ser válida.");
  }
}
