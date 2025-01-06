using System;

namespace reymani_web_api.Api.Endpoints.Direccion;

public class CreateDireccionRequest
{
  public required string TipoEntidad { get; set; }
  public required Guid IdEntidad { get; set; }
  public required string DireccionEntidad { get; set; }
  public string? Municipio { get; set; }
  public string? Provincia { get; set; }
  public double Latitud { get; set; }
  public double Longitud { get; set; }
  public string? Descripcion { get; set; }
}

public class CreateDireccionRequestValidator : Validator<CreateDireccionRequest>
{
  public CreateDireccionRequestValidator()
  {
    RuleFor(x => x.TipoEntidad)
      .NotEmpty().WithMessage("El tipo de entidad es obligatorio.")
      .MaximumLength(50).WithMessage("El tipo de entidad no puede tener más de 50 caracteres.")
      .Matches("^[a-zA-ZáéíóúÁÉÍÓÚñÑ ]+$").WithMessage("El tipo de entidad solo puede contener letras y espacios.");

    RuleFor(x => x.IdEntidad)
      .NotEmpty().WithMessage("El ID de la entidad es obligatorio.")
      .NotEqual(Guid.Empty).WithMessage("El ID de la entidad no puede ser vacío.");

    RuleFor(x => x.DireccionEntidad)
      .NotEmpty().WithMessage("La dirección es obligatoria.")
      .MaximumLength(255).WithMessage("La dirección no puede tener más de 255 caracteres.");

    RuleFor(x => x.Municipio)
      .MaximumLength(100).WithMessage("El municipio no puede tener más de 100 caracteres.")
      .Matches("^[a-zA-ZáéíóúÁÉÍÓÚñÑ ]+$").WithMessage("El municipio solo puede contener letras y espacios.");

    RuleFor(x => x.Provincia)
      .MaximumLength(100).WithMessage("La provincia no puede tener más de 100 caracteres.")
      .Matches("^[a-zA-ZáéíóúÁÉÍÓÚñÑ ]+$").WithMessage("La provincia solo puede contener letras y espacios.");

    RuleFor(x => x.Descripcion)
      .MaximumLength(255).WithMessage("La descripción no puede tener más de 255 caracteres.")
      ;
  }
}
