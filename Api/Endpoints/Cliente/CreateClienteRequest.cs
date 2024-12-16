using System;
using System.Globalization;

namespace reymani_web_api.Api.Endpoints.Cliente;

public class CreateClienteRequest
{
  public required string NumeroCarnet { get; set; }
  public required string Nombre { get; set; }
  public required string Apellidos { get; set; }
  public required string Username { get; set; }
  public required string Password { get; set; }
}

public class CreateClienteRequestValidator : Validator<CreateClienteRequest>
{
  public CreateClienteRequestValidator()
  {
    RuleFor(x => x.NumeroCarnet)
      .NotEmpty().WithMessage("El Número de Carnet es obligatorio.")
      .Length(11).WithMessage("El Número de Carnet debe tener 11 dígitos.")
      .Matches("^[0-9]*$").WithMessage("El Número de Carnet debe contener solo dígitos.")
      .Must(BeAValidDate).WithMessage("Los primeros 6 dígitos del Número de Carnet deben corresponder a una fecha válida.");

    RuleFor(x => x.Nombre)
      .NotEmpty().WithMessage("El Nombre es obligatorio.")
      .Length(3, 50).WithMessage("El Nombre debe tener entre 3 y 50 caracteres.")
      .Matches("^[a-zA-ZáéíóúÁÉÍÓÚñÑ ]*$").WithMessage("El Nombre solo puede contener letras y espacios.");

    RuleFor(x => x.Apellidos)
      .NotEmpty().WithMessage("Los Apellidos son obligatorios.")
      .Length(10, 100).WithMessage("Los Apellidos deben tener entre 10 y 100 caracteres.")
      .Matches("^[a-zA-ZáéíóúÁÉÍÓÚñÑ ]*$").WithMessage("Los Apellidos solo pueden contener letras y espacios.");

    RuleFor(x => x.Username)
      .NotEmpty().WithMessage("El Nombre de Usuario es obligatorio.")
      .Length(5, 25).WithMessage("El Nombre de Usuario debe tener entre 5 y 25 caracteres.")
      .Matches("^[a-zA-Z0-9]*$").WithMessage("El Nombre de Usuario solo puede contener letras y números.");

    RuleFor(x => x.Password)
      .NotEmpty().WithMessage("La Contraseña es obligatoria.")
      .Length(8, 25).WithMessage("La Contraseña debe tener entre 8 y 50 caracteres.")
      .Matches("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d).+$").WithMessage("La Contraseña debe contener al menos una letra minúscula, una letra mayúscula y un dígito.");
  }

  private bool BeAValidDate(string numeroCarnet)
  {
    if (numeroCarnet.Length < 6)
      return false;

    var datePart = numeroCarnet.Substring(0, 6);
    if (DateTime.TryParseExact(datePart, "yyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var date))
    {
      return date < DateTime.Now;
    }
    return false;
  }
}

