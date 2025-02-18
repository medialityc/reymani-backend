using System;

using FastEndpoints;

using FluentValidation;

namespace reymani_web_api.Endpoints.Auth.Requests.Validators
{
  public class ConfirmEndpointRequestValidator : Validator<ConfirmEndpointRequest>
  {
    public ConfirmEndpointRequestValidator()
    {
      RuleFor(x => x.ConfirmationCode)
        .NotEmpty().WithMessage("El código es requerido")
        .Length(4).WithMessage("El código debe contener 4 dígitos")
        .Matches(@"^\d{4}$").WithMessage("El código de confirmación debe ser numérico de 4 dígitos");

      RuleFor(x => x.Email)
        .NotEmpty().WithMessage("El correo es requerido")
        .EmailAddress().WithMessage("El correo no es válido");
    }
  }
}