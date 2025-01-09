namespace reymani_web_api.Application.DTOs;

public class CostoEnvioDto
{
  public Guid IdCostoEnvio { get; set; }
  public Guid IdNegocio { get; set; }
  public int DistanciaMaxKm { get; set; }
  public decimal Costo { get; set; }
}

public class CostoEnvioDtoValidator : Validator<CostoEnvioDto>
{
  public CostoEnvioDtoValidator()
  {
    RuleFor(x => x.IdCostoEnvio).NotEmpty().WithMessage("El costo de envío es requerido.");
    RuleFor(x => x.IdNegocio).NotEmpty().WithMessage("El negocio es requerido.");
    RuleFor(x => x.DistanciaMaxKm).GreaterThan(0).WithMessage("La distancia máxima debe ser mayor que 0.");
    RuleFor(x => x.Costo).GreaterThan(0).WithMessage("El costo debe ser mayor que 0.");
  }
}
