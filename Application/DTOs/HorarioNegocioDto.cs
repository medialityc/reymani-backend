namespace reymani_web_api.Application.DTOs;

public class HorarioNegocioDto
{
  public Guid IdHorario { get; set; }
  public Guid IdNegocio { get; set; }
  public int Dia { get; set; }
  public TimeSpan HoraApertura { get; set; }
  public TimeSpan HoraCierre { get; set; }
}

public class HorarioNegocioDtoValidator : Validator<HorarioNegocioDto>
{
  public HorarioNegocioDtoValidator()
  {
    RuleFor(x => x.IdNegocio).NotEmpty().WithMessage("El Id del negocio es requerido");
    RuleFor(x => x.Dia).InclusiveBetween(1, 7).WithMessage("El día debe estar entre 1 y 7");
    RuleFor(x => x.HoraApertura).NotEmpty().WithMessage("La hora de apertura es requerida");
    RuleFor(x => x.HoraCierre).NotEmpty().WithMessage("La hora de cierre es requerida");
  }
}

