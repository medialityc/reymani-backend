using FastEndpoints;

using FluentValidation;

namespace reymani_web_api.Endpoints.Vehicles.Requests.Validators;

public class DeleteVehicleRequestValidator : Validator<DeleteVehicleRequest>
{
  public DeleteVehicleRequestValidator()
  {
    RuleFor(e => e.Id)
      .NotEmpty()
      .GreaterThan(0);
  }
}
