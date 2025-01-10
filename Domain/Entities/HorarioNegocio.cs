using System.Text.Json.Serialization;

namespace reymani_web_api.Domain.Entities;

public class HorarioNegocio
{
  public Guid IdHorario { get; set; }  // PK
  public Guid IdNegocio { get; set; }  // FK
  public int Dia { get; set; }     // Día de la semana (Ej: "1 Lunes", "2 Martes", etc.)
  public TimeSpan HoraApertura { get; set; } // Hora de apertura (Ej: 08:00 AM)
  public TimeSpan HoraCierre { get; set; }  // Hora de cierre (Ej: 08:00 PM)

  // Relación de muchos a uno con Negocio
  [JsonIgnore]
  public Negocio? Negocio { get; set; }
}

