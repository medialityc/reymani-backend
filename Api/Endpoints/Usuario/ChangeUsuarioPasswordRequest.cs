using System;

namespace reymani_web_api.Api.Endpoints.Usuario;

public class ChangeUsuarioPasswordRequest
{
  public Guid UsuarioId { get; set; }
  public required string Password { get; set; }
  public required string NewPassword { get; set; }
}

public class ChangeUsuarioPasswordRequestValidator : Validator<ChangeUsuarioPasswordRequest>
{
  public ChangeUsuarioPasswordRequestValidator()
  {
    RuleFor(x => x.Password).NotEmpty().WithMessage("La Contraseña es obligatoria.");

    RuleFor(x => x.NewPassword).NotEmpty().WithMessage("La Contraseña es obligatoria.")
      .Length(8, 25).WithMessage("La Contraseña debe tener entre 8 y 50 caracteres.")
      .Matches("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d).+$").WithMessage("La Contraseña debe contener al menos una letra minúscula, una letra mayúscula y un dígito.");
  }
}