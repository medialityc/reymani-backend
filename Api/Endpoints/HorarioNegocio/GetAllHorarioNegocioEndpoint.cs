using System;
using reymani_web_api.Application.DTOs;

namespace reymani_web_api.Api.Endpoints.HorarioNegocio;

public sealed class GetAllHorarioNegocioEndpoint : EndpointWithoutRequest<GetAllHorarioNegocioResponse>
{
  private readonly IHorarioNegocioService _horarioNegocioService;
  private readonly IAuthorizationService _authorizationService;

  public GetAllHorarioNegocioEndpoint(IHorarioNegocioService horarioNegocioService, IAuthorizationService authorizationService)
  {
    _horarioNegocioService = horarioNegocioService;
    _authorizationService = authorizationService;
  }

  public override void Configure()
  {
    Get("/horario-negocio");
    Summary(s =>
    {
      s.Summary = "Obtener todos los horarios de negocio";
      s.Description = "Obtiene todos los horarios de negocio de la base de datos";
      s.ResponseExamples[200] = new GetAllHorarioNegocioResponse
      {
        HorariosNegocios = new List<HorarioNegocioDto>
        {
          new HorarioNegocioDto
          {
            IdHorario = Guid.NewGuid(),
            IdNegocio = Guid.NewGuid(),
            Dia = 1,
            HoraApertura = new TimeSpan(8, 0, 0),
            HoraCierre = new TimeSpan(20, 0, 0)
          }
        }
      };
    });
  }

  public override async Task<GetAllHorarioNegocioResponse> ExecuteAsync(CancellationToken ct)
  {
    var roleGuids = User.Claims.Where(c => c.Type == "role").Select(c => Guid.Parse(c.Value)).ToArray();

    if (!await _authorizationService.IsRoleAuthorizedToEndpointAsync(roleGuids, "Ver_Horarios_Negocio"))
    {
      await SendUnauthorizedAsync(ct);
    }

    var horariosNegocios = await _horarioNegocioService.GetAllAsync();
    return new GetAllHorarioNegocioResponse
    {
      HorariosNegocios = horariosNegocios.Select(h => new HorarioNegocioDto
      {
        IdHorario = h.IdHorario,
        IdNegocio = h.IdNegocio,
        Dia = h.Dia,
        HoraApertura = h.HoraApertura,
        HoraCierre = h.HoraCierre
      }).ToList()
    };
  }
}
