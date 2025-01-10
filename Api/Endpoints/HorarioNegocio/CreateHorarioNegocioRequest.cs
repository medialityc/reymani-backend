namespace reymani_web_api.Api.Endpoints.HorarioNegocio;

public class CreateHorarioNegocioRequest
{
  public required Guid IdNegocio { get; set; }
  public required int Dia { get; set; }
  public required TimeSpan HoraApertura { get; set; }
  public required TimeSpan HoraCierre { get; set; }
}

public class CreateHorarioNegocioRequestValidator : Validator<CreateHorarioNegocioRequest>
{
  public CreateHorarioNegocioRequestValidator()
  {
    RuleFor(x => x.IdNegocio).NotEmpty().WithMessage("El Id del negocio es requerido");
    RuleFor(x => x.Dia).InclusiveBetween(1, 7).WithMessage("El día debe estar entre 1 y 7");
    RuleFor(x => x.HoraApertura).NotEmpty().WithMessage("La hora de apertura es requerida");
    RuleFor(x => x.HoraCierre).NotEmpty().WithMessage("La hora de cierre es requerida");
  }
}
