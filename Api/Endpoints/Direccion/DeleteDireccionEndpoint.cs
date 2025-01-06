using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using reymani_web_api.Application.Interfaces;

namespace reymani_web_api.Api.Endpoints.Direccion;

public sealed class DeleteDireccionEndpoint : Endpoint<DeleteDireccionRequest>
{
  private readonly IDireccionService _direccionService;
  private readonly IAuthorizationService _authorizationService;

  public DeleteDireccionEndpoint(IDireccionService direccionService, IAuthorizationService authorizationService)
  {
    _direccionService = direccionService;
    _authorizationService = authorizationService;
  }

  public override void Configure()
  {
    Verbs(Http.DELETE);
    Routes("/direccion/{IdDireccion:guid}");
    Summary(s =>
    {
      s.Summary = "Eliminar Direccion";
      s.Description = "Elimina una direccion de la base de datos";
      s.ExampleRequest = new DeleteDireccionRequest
      {
        IdDireccion = Guid.NewGuid()
      };
      s.Responses[404] = "Direccion no encontrada";
      s.Responses[200] = "Direccion eliminada";
      s.Responses[401] = "No autorizado";
    });
  }

  public override async Task HandleAsync(DeleteDireccionRequest req, CancellationToken ct)
  {
    var roleGuids = User.Claims.Where(c => c.Type == "role").Select(c => Guid.Parse(c.Value)).ToArray();

    if (!await _authorizationService.IsRoleAuthorizedToEndpointAsync(roleGuids, "Eliminar_Direccion"))
    {
      await SendUnauthorizedAsync(ct);
    }

    var direccion = await _direccionService.GetByIdAsync(req.IdDireccion);
    if (direccion == null)
    {
      await SendNotFoundAsync(ct);
      return;
    }

    await _direccionService.DeleteAsync(req.IdDireccion);
    await SendOkAsync(ct);
  }
}
