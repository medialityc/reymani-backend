using System;
using reymani_web_api.Domain.Enums;

namespace reymani_web_api.Api.Endpoints.Telefono;

public class CreateTelefonoRequest
{
  public required string Numero { get; set; }
  public required string TipoEntidad { get; set; }
  public required Guid IdEntidad { get; set; }
  public string? Descripcion { get; set; }
}

public class CreateTelefonoRequestValidator : Validator<CreateTelefonoRequest>
{
  public CreateTelefonoRequestValidator()
  {
    RuleFor(x => x.Numero)
      .NotEmpty().WithMessage("El número es obligatorio.")
      .Matches("^[0-9]+$").WithMessage("El número solo puede contener dígitos.");

    RuleFor(x => x.TipoEntidad)
      .NotEmpty().WithMessage("El tipo de entidad es obligatorio.");

    RuleFor(x => x.IdEntidad)
      .NotEmpty().WithMessage("El ID de la entidad es obligatorio.")
      .NotEqual(Guid.Empty).WithMessage("El ID de la entidad no puede ser vacío.");

    RuleFor(x => x.Descripcion)
      .MaximumLength(100).WithMessage("La descripción no puede tener más de 100 caracteres.")
      .Matches("^[a-zA-ZáéíóúÁÉÍÓÚñÑ0-9., ]*$").WithMessage("La descripción solo puede contener letras, números, espacios y signos de puntuación.");
  }
}
