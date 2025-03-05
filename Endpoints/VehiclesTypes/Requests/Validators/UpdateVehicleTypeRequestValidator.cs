using FastEndpoints;

using FluentValidation;

using reymani_web_api.Endpoints.Vehicles.Requests;
using reymani_web_api.Utils.Validations;

namespace reymani_web_api.Endpoints.VehiclesTypes.Requests.Validators;

public class UpdateVehicleTypeRequestValidator : Validator<UpdateVehicleTypeRequest>
{
  public UpdateVehicleTypeRequestValidator()
  {
    RuleFor(e => e.Id)
      .NotEmpty()
      .GreaterThan(0);

    RuleFor(x => x.Logo)
       .Must(file => file == null || (ImageValidations.BeAValidImage(file) && ImageValidations.HaveValidLength(file)))
       .WithMessage("La imagen debe ser válida.");
  }
}
