using System;

namespace reymani_web_api.Api.Endpoints.Telefono;

public class UpdateTelefonoRequest
{
  public Guid IdTelefono { get; set; }
  public required Domain.Entities.Telefono Telefono { get; set; }
}

public class UpdateTelefonoRequestValidator : Validator<UpdateTelefonoRequest>
{
  public UpdateTelefonoRequestValidator()
  {
    RuleFor(x => x.Telefono.IdTelefono).NotEmpty().WithMessage("El ID del teléfono es requerido")
      .Must((request, idTelefono) => idTelefono == request.IdTelefono).WithMessage("El ID del teléfono no coincide con el ID del teléfono a actualizar");

    RuleFor(x => x.Telefono.NumeroTelefono)
      .NotEmpty().WithMessage("El número de teléfono es requerido")
      .Matches("^[0-9]+$").WithMessage("El número solo puede contener dígitos.");

    RuleFor(x => x.Telefono.IdEntidad)
      .NotEmpty().WithMessage("El ID de la entidad es requerido")
      .NotEqual(Guid.Empty).WithMessage("El ID de la entidad no puede ser vacío.");

    RuleFor(x => x.Telefono.Descripcion)
      .MaximumLength(100).WithMessage("La descripción no puede tener más de 100 caracteres.")
      .Matches("^[a-zA-ZáéíóúÁÉÍÓÚñÑ0-9., ]*$").WithMessage("La descripción solo puede contener letras, números, espacios y signos de puntuación.");
  }
}
