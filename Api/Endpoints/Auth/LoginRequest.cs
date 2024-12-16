using System;

namespace reymani_web_api.Api.Endpoints.Auth;

public class LoginRequest
{
  public required string UsernameOrPhone { get; set; }
  public required string Password { get; set; }
}


public class LoginRequestValidator : AbstractValidator<LoginRequest>
{
  public LoginRequestValidator()
  {
    RuleFor(x => x.UsernameOrPhone)
      .NotEmpty().WithMessage("El Nombre de Usuario o Teléfono es obligatorio.")
      .Length(5, 25).WithMessage("El Nombre de Usuario o Teléfono debe tener entre 5 y 25 caracteres.")
      .Matches("^[a-zA-Z0-9]*$").WithMessage("El Nombre de Usuario solo puede contener letras y números.");

    RuleFor(x => x.Password)
      .NotEmpty().WithMessage("La Contraseña es obligatoria.")
      .Length(8, 25).WithMessage("La Contraseña debe tener entre 8 y 50 caracteres.")
      .Matches("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d).+$").WithMessage("La Contraseña debe contener al menos una letra minúscula, una letra mayúscula y un dígito.");
  }
}