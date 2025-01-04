using System;

namespace reymani_web_api.Api.Endpoints.Telefono;

public sealed class DeleteTelefonoEndpoint : Endpoint<DeleteTelefonoRequest>
{
  private readonly ITelefonoService _telefonoService;
  private readonly IAuthorizationService _authorizationService;

  public DeleteTelefonoEndpoint(ITelefonoService telefonoService, IAuthorizationService authorizationService)
  {
    _telefonoService = telefonoService;
    _authorizationService = authorizationService;
  }

  public override void Configure()
  {
    Verbs(Http.DELETE);
    Routes("/telefono/{IdTelefono:guid}");
    Summary(s =>
    {
      s.Summary = "Eliminar Telefono";
      s.Description = "Elimina un telefono de la base de datos";
      s.ExampleRequest = new DeleteTelefonoRequest
      {
        IdTelefono = Guid.NewGuid()
      };
      s.Responses[404] = "Telefono no encontrado";
      s.Responses[200] = "Telefono eliminado";
      s.Responses[401] = "No autorizado";
    });
  }

  public override async Task HandleAsync(DeleteTelefonoRequest req, CancellationToken ct)
  {
    var roleGuids = User.Claims.Where(c => c.Type == "role").Select(c => Guid.Parse(c.Value)).ToArray();

    if (!await _authorizationService.IsRoleAuthorizedToEndpointAsync(roleGuids, "Eliminar_Telefono"))
    {
      await SendUnauthorizedAsync(ct);
    }

    var telefono = await _telefonoService.GetByIdAsync(req.IdTelefono);
    if (telefono == null)
    {
      await SendNotFoundAsync(ct);
      return;
    }

    await _telefonoService.DeleteAsync(req.IdTelefono);
    await SendOkAsync(ct);
  }
}
