namespace reymani_web_api.Api.Endpoints.HorarioNegocio;

public sealed class CreateHorarioNegocioEndpoint : Endpoint<CreateHorarioNegocioRequest>
{
  private readonly IHorarioNegocioService _horarioNegocioService;
  private readonly IAuthorizationService _authorizationService;
  private readonly INegocioService _negocioService;

  public CreateHorarioNegocioEndpoint(IHorarioNegocioService horarioNegocioService, IAuthorizationService authorizationService, INegocioService negocioService)
  {
    _horarioNegocioService = horarioNegocioService;
    _authorizationService = authorizationService;
    _negocioService = negocioService;
  }

  public override void Configure()
  {
    Verbs(Http.POST);
    Routes("/horario-negocio");
    Summary(s =>
    {
      s.Summary = "Crear Horario de Negocio";
      s.Description = "Crea un nuevo horario de negocio";
      s.ExampleRequest = new CreateHorarioNegocioRequest
      {
        IdNegocio = Guid.NewGuid(),
        Dia = 1,
        HoraApertura = new TimeSpan(8, 0, 0),
        HoraCierre = new TimeSpan(20, 0, 0)
      };
    });
  }

  public override async Task HandleAsync(CreateHorarioNegocioRequest req, CancellationToken ct)
  {
    var roleGuids = User.Claims.Where(c => c.Type == "role").Select(c => Guid.Parse(c.Value)).ToArray();

    if (!await _authorizationService.IsRoleAuthorizedToEndpointAsync(roleGuids, "Crear_Horario_Negocio"))
    {
      await SendUnauthorizedAsync(ct);
    }

    if (req.HoraApertura >= req.HoraCierre)
    {
      AddError(req => req.HoraApertura, "La hora de apertura debe ser menor a la hora de cierre");
    }

    var negocio = await _negocioService.GetByIdAsync(req.IdNegocio);
    if (negocio == null)
    {
      AddError(req => req.IdNegocio, "Negocio no encontrado");
    }

    if (await _horarioNegocioService.HorarioExistsForDiaAsync(req.IdNegocio, req.Dia))
    {
      AddError(req => req.Dia, "Ya existe un horario para este día");
    }

    ThrowIfAnyErrors();

    var horarioNegocio = new Domain.Entities.HorarioNegocio
    {
      IdHorario = Guid.NewGuid(),
      IdNegocio = req.IdNegocio,
      Dia = req.Dia,
      HoraApertura = req.HoraApertura,
      HoraCierre = req.HoraCierre
    };

    await _horarioNegocioService.AddAsync(horarioNegocio);

    await SendOkAsync(horarioNegocio, ct);
  }
}
