namespace reymani_web_api.Api.Endpoints.CostoEnvio;

public sealed class DeleteCostoEnvioEndpoint : Endpoint<DeleteCostoEnvioRequest>
{
  private readonly ICostoEnvioService _costoEnvioService;
  private readonly IAuthorizationService _authorizationService;

  public DeleteCostoEnvioEndpoint(ICostoEnvioService costoEnvioService, IAuthorizationService authorizationService)
  {
    _costoEnvioService = costoEnvioService;
    _authorizationService = authorizationService;
  }

  public override void Configure()
  {
    Verbs(Http.DELETE);
    Routes("/costo-envio/{CostoEnvioId}");
    Summary(s =>
    {
      s.Summary = "Eliminar Costo de Envío";
      s.Description = "Elimina un Costo de Envío";
      s.ExampleRequest = new DeleteCostoEnvioRequest
      {
        CostoEnvioId = Guid.NewGuid()
      };
    });
  }

  public override async Task HandleAsync(DeleteCostoEnvioRequest req, CancellationToken ct)
  {
    var roleGuids = User.Claims.Where(c => c.Type == "role").Select(c => Guid.Parse(c.Value)).ToArray();

    if (!await _authorizationService.IsRoleAuthorizedToEndpointAsync(roleGuids, "Eliminar_Costo_Envio"))
    {
      await SendUnauthorizedAsync(ct);
    }

    var costoEnvio = await _costoEnvioService.GetByIdAsync(req.CostoEnvioId);
    if (costoEnvio == null)
    {
      await SendNotFoundAsync(ct);
      return;
    }

    await _costoEnvioService.DeleteAsync(req.CostoEnvioId);
    await SendOkAsync(ct);
  }
}
