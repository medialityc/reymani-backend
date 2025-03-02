using FastEndpoints;

using FluentValidation;

using reymani_web_api.Utils.Validations;

namespace reymani_web_api.Endpoints.Vehicles.Requests.Validators;

public class CreateVehicleCourierRequestValidator : Validator<CreateVehicleCourierRequest>
{
  public CreateVehicleCourierRequestValidator()
  {
    RuleFor(x => x.Picture)
           .Must(file => file == null || (ImageValidations.BeAValidImage(file) && ImageValidations.HaveValidLength(file)))
           .WithMessage("La imagen debe ser válida.");

    RuleFor(e => e.Name)
      .NotEmpty();

    RuleFor(e => e.VehicleTypeId)
      .NotEmpty()
      .GreaterThan(0);
  }
}
