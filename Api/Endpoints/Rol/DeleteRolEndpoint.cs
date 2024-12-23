using System;

namespace reymani_web_api.Api.Endpoints.Rol;

public sealed class DeleteRolEndpoint : Endpoint<DeleteRolRequest>
{
  private readonly IRolService _rolService;

  public DeleteRolEndpoint(IRolService rolService)
  {
    _rolService = rolService;
  }

  public override void Configure()
  {
    Verbs(Http.DELETE);
    Routes("/Rol/{RolId}");
    Summary(s =>
    {
      s.Summary = "Eliminar Rol";
      s.Description = "Elimina un Rol";
      s.ExampleRequest = new DeleteRolRequest
      {
        RolId = Guid.NewGuid()
      };
    });
  }

  public override async Task HandleAsync(DeleteRolRequest req, CancellationToken ct)
  {
    var rol = await _rolService.GetByIdAsync(req.RolId);
    if (rol == null)
    {
      await SendNotFoundAsync(ct);
      return;
    }

    await _rolService.DeleteAsync(req.RolId);
    await SendOkAsync(ct);
  }
}