namespace reymani_web_api.Application.DTOs;

public class NegocioDto
{
  public Guid IdNegocio { get; set; }
  public required string Nombre { get; set; }
  public string? Descripcion { get; set; }
  public bool EntregaDomicilio { get; set; }
}

public class NegocioDtoValidator : Validator<NegocioDto>
{
  public NegocioDtoValidator()
  {
    RuleFor(x => x.Nombre)
      .NotEmpty().WithMessage("El nombre es obligatorio.")
      .MaximumLength(100).WithMessage("El nombre no puede tener más de 100 caracteres.")
      .Matches("^[a-zA-ZáéíóúÁÉÍÓÚñÑ ]*$").WithMessage("El nombre solo puede contener letras y espacios.");

    RuleFor(x => x.Descripcion)
      .NotEmpty().WithMessage("La descripción es obligatoria.")
      .MaximumLength(500).WithMessage("La descripción no puede tener más de 500 caracteres.")
      .Matches("^[a-zA-ZáéíóúÁÉÍÓÚñÑ0-9., ]*$").WithMessage("La descripción solo puede contener letras, números, espacios y signos de puntuación.");

    RuleFor(x => x.EntregaDomicilio)
      .NotNull().WithMessage("El campo entrega a domicilio es obligatorio.");

  }
}
