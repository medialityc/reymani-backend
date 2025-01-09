namespace reymani_web_api.Api.Endpoints.CostoEnvio;

public class CreateCostoEnvioRequest
{
  public required Guid IdNegocio { get; set; }
  public required int DistanciaMaxKm { get; set; }
  public required decimal Costo { get; set; }
}

public class CreateCostoEnvioRequestValidator : Validator<CreateCostoEnvioRequest>
{
  public CreateCostoEnvioRequestValidator()
  {
    RuleFor(x => x.IdNegocio).NotEmpty().WithMessage("El Id del negocio es requerido");

    RuleFor(x => x.DistanciaMaxKm)
      .GreaterThan(0).WithMessage("La distancia máxima en km debe ser mayor a 0");

    RuleFor(x => x.Costo)
      .GreaterThan(0).WithMessage("El costo debe ser mayor a 0");
  }
}
