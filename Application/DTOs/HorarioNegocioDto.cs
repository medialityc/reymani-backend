namespace reymani_web_api.Application.DTOs;

public class HorarioNegocioDto
{
  public Guid IdHorario { get; set; }
  public Guid IdNegocio { get; set; }
  public int Dia { get; set; }
  public TimeSpan HoraApertura { get; set; }
  public TimeSpan HoraCierre { get; set; }
}

