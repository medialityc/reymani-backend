using reymani_web_api.Application.DTOs;

namespace reymani_web_api.Api.Endpoints.HorarioNegocio;

public class GetAllHorarioNegocioByIdNegocioEndpoint : Endpoint<GetAllHorarioNegocioByIdNegocioRequest, GetAllHorarioNegocioByIdNegocioResponse>
{
  private readonly IHorarioNegocioService _horarioNegocioService;
  private readonly INegocioService _negocioService;
  private readonly IAuthorizationService _authorizationService;

  public GetAllHorarioNegocioByIdNegocioEndpoint(IHorarioNegocioService horarioNegocioService, INegocioService negocioService, IAuthorizationService authorizationService)
  {
    _horarioNegocioService = horarioNegocioService;
    _negocioService = negocioService;
    _authorizationService = authorizationService;
  }

  public override void Configure()
  {
    Get("/horario-negocio/negocio/{IdNegocio}");
    Summary(s =>
    {
      s.Summary = "Obtener todos los horarios de negocio por IdNegocio";
      s.Description = "Obtiene todos los horarios de negocio asociados a un negocio específico";
      s.ResponseExamples[200] = new GetAllHorarioNegocioByIdNegocioResponse
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

  public override async Task<GetAllHorarioNegocioByIdNegocioResponse> ExecuteAsync(GetAllHorarioNegocioByIdNegocioRequest req, CancellationToken ct)
  {
    var roleGuids = User.Claims.Where(c => c.Type == "role").Select(c => Guid.Parse(c.Value)).ToArray();

    if (!await _authorizationService.IsRoleAuthorizedToEndpointAsync(roleGuids, "Ver_Horarios_Negocio_Negocio"))
    {
      await SendUnauthorizedAsync(ct);
    }

    var negocio = await _negocioService.GetByIdAsync(req.IdNegocio);
    if (negocio == null)
    {
      AddError(r => r.IdNegocio, "Negocio no encontrado");
    }

    ThrowIfAnyErrors();

    var horariosNegocios = await _horarioNegocioService.GetAllAsync();
    var filteredHorariosNegocios = horariosNegocios.Where(hn => hn.IdNegocio == req.IdNegocio);

    return new GetAllHorarioNegocioByIdNegocioResponse
    {
      HorariosNegocios = filteredHorariosNegocios.Select(hn => new HorarioNegocioDto
      {
        IdHorario = hn.IdHorario,
        IdNegocio = hn.IdNegocio,
        Dia = hn.Dia,
        HoraApertura = hn.HoraApertura,
        HoraCierre = hn.HoraCierre
      }).ToList()
    };
  }
}
