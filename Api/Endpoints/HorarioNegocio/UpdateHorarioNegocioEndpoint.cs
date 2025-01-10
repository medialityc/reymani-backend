using System;
using reymani_web_api.Application.DTOs;

namespace reymani_web_api.Api.Endpoints.HorarioNegocio;

public sealed class UpdateHorarioNegocioEndpoint : Endpoint<UpdateHorarioNegocioRequest>
{
  private readonly IHorarioNegocioService _horarioNegocioService;
  private readonly IAuthorizationService _authorizationService;
  private readonly INegocioService _negocioService;

  public UpdateHorarioNegocioEndpoint(IHorarioNegocioService horarioNegocioService, IAuthorizationService authorizationService, INegocioService negocioService)
  {
    _horarioNegocioService = horarioNegocioService;
    _authorizationService = authorizationService;
    _negocioService = negocioService;

  }

  public override void Configure()
  {
    Verbs(Http.PUT);
    Routes("/horario-negocio/{IdHorario:guid}");
    Summary(s =>
    {
      s.Summary = "Actualizar Horario de Negocio";
      s.Description = "Actualiza un horario de negocio";
      s.ExampleRequest = new UpdateHorarioNegocioRequest
      {
        IdHorario = Guid.NewGuid(),
        HorarioNegocio = new HorarioNegocioDto
        {
          IdNegocio = Guid.NewGuid(),
          Dia = 1,
          HoraApertura = new TimeSpan(8, 0, 0),
          HoraCierre = new TimeSpan(20, 0, 0)
        }
      };
    });
  }

  public override async Task HandleAsync(UpdateHorarioNegocioRequest req, CancellationToken ct)
  {
    var roleGuids = User.Claims.Where(c => c.Type == "role").Select(c => Guid.Parse(c.Value)).ToArray();

    if (!await _authorizationService.IsRoleAuthorizedToEndpointAsync(roleGuids, "Actualizar_Horario_Negocio"))
    {
      await SendUnauthorizedAsync(ct);
      return;
    }

    var horarioNegocio = await _horarioNegocioService.GetByIdAsync(req.IdHorario);
    if (horarioNegocio == null)
    {
      await SendNotFoundAsync(ct);
      return;
    }

    if (req.HorarioNegocio.IdHorario != req.IdHorario)
    {
      AddError(r => r.IdHorario, "El ID del horario de negocio no coincide con el ID de la URL");
    }

    var negocio = await _negocioService.GetByIdAsync(req.HorarioNegocio.IdNegocio);
    if (negocio == null)
    {
      AddError(r => r.HorarioNegocio.IdNegocio, "El negocio no existe");
    }

    if (req.HorarioNegocio.HoraApertura >= req.HorarioNegocio.HoraCierre)
    {
      AddError(req => req.HorarioNegocio.HoraApertura, "La hora de apertura debe ser menor a la hora de cierre");
    }

    ThrowIfAnyErrors();

    var updatedHorarioNegocio = new Domain.Entities.HorarioNegocio
    {
      IdHorario = req.IdHorario,
      IdNegocio = req.HorarioNegocio.IdNegocio,
      Dia = req.HorarioNegocio.Dia,
      HoraApertura = req.HorarioNegocio.HoraApertura,
      HoraCierre = req.HorarioNegocio.HoraCierre
    };

    await _horarioNegocioService.UpdateAsync(updatedHorarioNegocio);

    await SendOkAsync(updatedHorarioNegocio, ct);
  }
}
