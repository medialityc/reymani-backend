
namespace reymani_web_api.Api.Endpoints.MetodoPago;

public sealed class DeleteMetodoPagoEndpoint : Endpoint<DeleteMetodoPagoRequest>
{
  private readonly IMetodoPagoService _metodoPagoService;
  private readonly IAuthorizationService _authorizationService;

  public DeleteMetodoPagoEndpoint(IMetodoPagoService metodoPagoService, IAuthorizationService authorizationService)
  {
    _metodoPagoService = metodoPagoService;
    _authorizationService = authorizationService;
  }

  public override void Configure()
  {
    Verbs(Http.DELETE);
    Routes("/metodo-pago/{IdMetodoPago:guid}");
    Summary(s =>
    {
      s.Summary = "Eliminar Método de Pago";
      s.Description = "Elimina un método de pago de la base de datos";
      s.ExampleRequest = new DeleteMetodoPagoRequest
      {
        IdMetodoPago = Guid.NewGuid()
      };
      s.Responses[404] = "Método de pago no encontrado";
      s.Responses[200] = "Método de pago eliminado";
      s.Responses[401] = "No autorizado";
    });
  }

  public override async Task HandleAsync(DeleteMetodoPagoRequest req, CancellationToken ct)
  {
    var roleGuids = User.Claims.Where(c => c.Type == "role").Select(c => Guid.Parse(c.Value)).ToArray();

    if (!await _authorizationService.IsRoleAuthorizedToEndpointAsync(roleGuids, "Eliminar_Metodo_Pago"))
    {
      await SendUnauthorizedAsync(ct);
    }

    var metodoPago = await _metodoPagoService.GetByIdAsync(req.IdMetodoPago);
    if (metodoPago == null)
    {
      await SendNotFoundAsync(ct);
      return;
    }

    await _metodoPagoService.DeleteAsync(req.IdMetodoPago);
    await SendOkAsync(ct);
  }
}