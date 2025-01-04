using System;

namespace reymani_web_api.Api.Endpoints.Negocio;

public sealed class DeleteNegocioEndpoint : Endpoint<DeleteNegocioRequest>
{
  private readonly INegocioService _negocioService;
  private readonly IAuthorizationService _authorizationService;

  public DeleteNegocioEndpoint(INegocioService negocioService, IAuthorizationService authorizationService)
  {
    _negocioService = negocioService;
    _authorizationService = authorizationService;
  }

  public override void Configure()
  {
    Verbs(Http.DELETE);
    Routes("/negocio/{IdNegocio:guid}");
    Summary(s =>
    {
      s.Summary = "Eliminar Negocio";
      s.Description = "Elimina un negocio de la base de datos";
      s.ExampleRequest = new DeleteNegocioRequest
      {
        IdNegocio = Guid.NewGuid()
      };
    });
  }

  public override async Task HandleAsync(DeleteNegocioRequest req, CancellationToken ct)
  {
    var roleGuids = User.Claims.Where(c => c.Type == "role").Select(c => Guid.Parse(c.Value)).ToArray();

    if (!await _authorizationService.IsRoleAuthorizedToEndpointAsync(roleGuids, "Eliminar_Negocio"))
    {
      await SendUnauthorizedAsync(ct);
    }

    var negocio = await _negocioService.GetByIdAsync(req.IdNegocio);
    if (negocio == null)
    {
      await SendNotFoundAsync(ct);
      return;
    }

    await _negocioService.DeleteAsync(req.IdNegocio);
    await SendOkAsync(ct);
  }
}
