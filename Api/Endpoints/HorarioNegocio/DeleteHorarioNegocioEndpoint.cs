using System;

namespace reymani_web_api.Api.Endpoints.HorarioNegocio
{
  public sealed class DeleteHorarioNegocioEndpoint : Endpoint<DeleteHorarioNegocioRequest>
  {
    private readonly IHorarioNegocioService _horarioNegocioService;
    private readonly IAuthorizationService _authorizationService;

    public DeleteHorarioNegocioEndpoint(IHorarioNegocioService horarioNegocioService, IAuthorizationService authorizationService)
    {
      _horarioNegocioService = horarioNegocioService;
      _authorizationService = authorizationService;
    }

    public override void Configure()
    {
      Verbs(Http.DELETE);
      Routes("/horario-negocio/{HorarioNegocioId}");
      Summary(s =>
      {
        s.Summary = "Eliminar Horario de Negocio";
        s.Description = "Elimina un Horario de Negocio";
        s.ExampleRequest = new DeleteHorarioNegocioRequest
        {
          HorarioNegocioId = Guid.NewGuid()
        };
      });
    }

    public override async Task HandleAsync(DeleteHorarioNegocioRequest req, CancellationToken ct)
    {
      var roleGuids = User.Claims.Where(c => c.Type == "role").Select(c => Guid.Parse(c.Value)).ToArray();

      if (!await _authorizationService.IsRoleAuthorizedToEndpointAsync(roleGuids, "Eliminar_Horario_Negocio"))
      {
        await SendUnauthorizedAsync(ct);
      }

      var horarioNegocio = await _horarioNegocioService.GetByIdAsync(req.HorarioNegocioId);
      if (horarioNegocio == null)
      {
        await SendNotFoundAsync(ct);
        return;
      }

      await _horarioNegocioService.DeleteAsync(req.HorarioNegocioId);
      await SendOkAsync(ct);
    }
  }
}
