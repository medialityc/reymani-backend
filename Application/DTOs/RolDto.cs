namespace reymani_web_api.Application.DTOs;

public class RolDto
{
  public Guid Id { get; set; } // PK, tipo Guid
  public required string Nombre { get; set; } // Nombre del rol (ej. 'Administrador', 'Usuario', 'Mensajero', etc.)
  public string? Descripcion { get; set; } // Descripción opcional del rol
}

public class RolDtoValidator : Validator<RolDto>
{
  public RolDtoValidator()
  {
    RuleFor(x => x.Nombre).NotEmpty().WithMessage("El nombre es requerido")
      .MaximumLength(50).WithMessage("El nombre no debe exceder los 50 caracteres")
      .Matches("^[a-zA-Z0-9 áéíóúÁÉÍÓÚñÑ]*$").WithMessage("El nombre solo puede contener letras, números y espacios");

    RuleFor(x => x.Descripcion).MaximumLength(100).WithMessage("La descripción no debe exceder los 100 caracteres")
      .Matches("^[a-zA-Z0-9 áéíóúÁÉÍÓÚñÑ]*$").WithMessage("La descripción solo puede contener letras, números y espacios")
      .When(x => x.Descripcion != null);
  }
}

