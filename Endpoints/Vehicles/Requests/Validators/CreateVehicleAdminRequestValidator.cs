using FastEndpoints;

using FluentValidation;

using reymani_web_api.Utils.Validations;

namespace reymani_web_api.Endpoints.Vehicles.Requests.Validators;

public class CreateVehicleAdminRequestValidator : Validator<CreateVehicleAdminRequest>
{
  public CreateVehicleAdminRequestValidator()
  {
    RuleFor(x => x.Picture)
           .Must(file => file == null || (ImageValidations.BeAValidImage(file) && ImageValidations.HaveValidLength(file)))
           .WithMessage("La imagen debe ser válida.");

    RuleFor(e => e.Name)
      .NotEmpty();

    RuleFor(e => e.IdCourier)
      .NotEmpty()
      .GreaterThan(0);

    RuleFor(e => e.VehicleTypeId)
      .NotEmpty()
      .GreaterThan(0);
  }
}
