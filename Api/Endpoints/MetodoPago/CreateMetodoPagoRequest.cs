
namespace reymani_web_api.Api.Endpoints.MetodoPago;

public class CreateMetodoPagoRequest
{
  public required string TipoEntidad { get; set; }
  public Guid IdEntidad { get; set; }
  public required string Proveedor { get; set; }
  public DateTime FechaExpiracion { get; set; }
  public string? Dato1 { get; set; }
  public string? Dato2 { get; set; }
  public string? Dato3 { get; set; }
}

public class CreateMetodoPagoRequestValidator : Validator<CreateMetodoPagoRequest>
{
  public CreateMetodoPagoRequestValidator()
  {
    RuleFor(x => x.TipoEntidad)
      .NotEmpty().WithMessage("El tipo de entidad es requerido.");

    RuleFor(x => x.IdEntidad)
      .NotEmpty().WithMessage("El ID de la entidad es requerido.");

    RuleFor(x => x.Proveedor)
      .NotEmpty().WithMessage("El proveedor es requerido.");

    RuleFor(x => x.FechaExpiracion)
      .GreaterThan(DateTime.UtcNow).WithMessage("La fecha de expiración debe ser una fecha futura.");

    RuleFor(x => x.Dato1)
      .MaximumLength(256).WithMessage("Dato1 no puede exceder los 256 caracteres.");

    RuleFor(x => x.Dato2)
      .MaximumLength(256).WithMessage("Dato2 no puede exceder los 256 caracteres.");

    RuleFor(x => x.Dato3)
      .MaximumLength(256).WithMessage("Dato3 no puede exceder los 256 caracteres.");
  }
}