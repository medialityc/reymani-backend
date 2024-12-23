using System;
using FluentValidation;
using reymani_web_api.Api.Utils;

namespace reymani_web_api.Application.DTOs;

public class ClienteDto
{
  public Guid Id { get; set; }
  public required string NumeroCarnet { get; set; }
  public required string Nombre { get; set; }
  public required string Apellidos { get; set; }
  public required string Username { get; set; }
}

public class ClienteDtoValidator : Validator<ClienteDto>
{
  public ClienteDtoValidator()
  {
    RuleFor(x => x.NumeroCarnet)
      .NotEmpty().WithMessage("El Número de Carnet es obligatorio.")
      .Length(11).WithMessage("El Número de Carnet debe tener 11 dígitos.")
      .Matches("^[0-9]*$").WithMessage("El Número de Carnet debe contener solo dígitos.")
      .Must(Validations.BeAValidDate).WithMessage("Los primeros 6 dígitos del Número de Carnet deben corresponder a una fecha válida.");

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
  }
}