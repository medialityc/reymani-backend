
using reymani_web_api.Application.DTOs;

namespace reymani_web_api.Api.Endpoints.HorarioNegocio;

public class GetAllHorarioNegocioByIdNegocioResponse
{
  public List<HorarioNegocioDto> HorariosNegocios { get; set; } = new List<HorarioNegocioDto>();
}
