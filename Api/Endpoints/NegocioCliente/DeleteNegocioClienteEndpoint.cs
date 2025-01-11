
namespace reymani_web_api.Api.Endpoints.NegocioCliente
{
  public sealed class DeleteNegocioClienteEndpoint : Endpoint<DeleteNegocioClienteRequest>
  {
    private readonly INegocioClienteService _negocioClienteService;
    private readonly IAuthorizationService _authorizationService;

    public DeleteNegocioClienteEndpoint(INegocioClienteService negocioClienteService, IAuthorizationService authorizationService)
    {
      _negocioClienteService = negocioClienteService;
      _authorizationService = authorizationService;
    }

    public override void Configure()
    {
      Verbs(Http.DELETE);
      Routes("/negocio-cliente/{NegocioClienteId}");
      Summary(s =>
      {
        s.Summary = "Eliminar NegocioCliente";
        s.Description = "Elimina un Negocio-Cliente. Es decir, se elimina la suscripcion del cliente a dicho negocio.";
        s.ExampleRequest = new DeleteNegocioClienteRequest
        {
          NegocioClienteId = Guid.NewGuid()
        };
      });
    }

    public override async Task HandleAsync(DeleteNegocioClienteRequest req, CancellationToken ct)
    {
      var roleGuids = User.Claims.Where(c => c.Type == "role").Select(c => Guid.Parse(c.Value)).ToArray();

      if (!await _authorizationService.IsRoleAuthorizedToEndpointAsync(roleGuids, "Eliminar_Negocio_Cliente"))
      {
        await SendUnauthorizedAsync(ct);
      }

      var negocioCliente = await _negocioClienteService.GetByIdAsync(req.NegocioClienteId);
      if (negocioCliente == null)
      {
        await SendNotFoundAsync(ct);
        return;
      }

      await _negocioClienteService.DeleteAsync(req.NegocioClienteId);
      await SendOkAsync(ct);
    }
  }
}
