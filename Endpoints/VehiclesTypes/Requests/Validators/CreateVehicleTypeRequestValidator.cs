using FastEndpoints;

using FluentValidation;

using reymani_web_api.Utils.Validations;

namespace reymani_web_api.Endpoints.VehiclesTypes.Requests.Validators;

public class CreateVehicleTypeRequestValidator : Validator<CreateVehicleTypeRequest>
{
  public CreateVehicleTypeRequestValidator()
  {
    RuleFor(e => e.Name)
      .NotEmpty();

    RuleFor(e => e.IsActive)
      .NotEmpty();

    RuleFor(e => e.TotalCapacity)
      .NotEmpty()
      .GreaterThan(0);

    RuleFor(x => x.Logo)
       .Must(file => (ImageValidations.BeAValidImage(file) && ImageValidations.HaveValidLength(file)))
       .WithMessage("La imagen debe ser válida.");
  }
}
