using System;

namespace reymani_web_api.Api.Endpoints.Direccion;

public class UpdateDireccionRequest
{
  public Guid IdDireccion { get; set; }
  public required Domain.Entities.Direccion Direccion { get; set; }
}

public class UpdateDireccionRequestValidator : Validator<UpdateDireccionRequest>
{
  public UpdateDireccionRequestValidator()
  {
    RuleFor(x => x.Direccion.IdDireccion).NotEmpty().WithMessage("El ID de la dirección es requerido")
      .Must((request, idDireccion) => idDireccion == request.IdDireccion).WithMessage("El ID de la dirección no coincide con el ID de la dirección a actualizar");

    RuleFor(x => x.Direccion.TipoEntidad)
       .NotEmpty().WithMessage("El tipo de entidad es obligatorio.")
       .MaximumLength(50).WithMessage("El tipo de entidad no puede tener más de 50 caracteres.")
       .Matches("^[a-zA-ZáéíóúÁÉÍÓÚñÑ ]+$").WithMessage("El tipo de entidad solo puede contener letras y espacios.");

    RuleFor(x => x.Direccion.IdEntidad)
      .NotEmpty().WithMessage("El ID de la entidad es obligatorio.")
      .NotEqual(Guid.Empty).WithMessage("El ID de la entidad no puede ser vacío.");

    RuleFor(x => x.Direccion.DireccionEntidad)
      .NotEmpty().WithMessage("La dirección es obligatoria.")
      .MaximumLength(255).WithMessage("La dirección no puede tener más de 255 caracteres.");

    RuleFor(x => x.Direccion.Municipio)
      .MaximumLength(100).WithMessage("El municipio no puede tener más de 100 caracteres.")
      .Matches("^[a-zA-ZáéíóúÁÉÍÓÚñÑ ]+$").WithMessage("El municipio solo puede contener letras y espacios.");

    RuleFor(x => x.Direccion.Provincia)
      .MaximumLength(100).WithMessage("La provincia no puede tener más de 100 caracteres.")
      .Matches("^[a-zA-ZáéíóúÁÉÍÓÚñÑ ]+$").WithMessage("La provincia solo puede contener letras y espacios.");

    RuleFor(x => x.Direccion.Descripcion)
      .MaximumLength(255).WithMessage("La descripción no puede tener más de 255 caracteres.");
  }
}
