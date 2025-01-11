
namespace reymani_web_api.Api.Endpoints.MetodoPago;

public class GetMetodoPagoByIdEndpoint : Endpoint<GetMetodoPagoByIdRequest, Domain.Entities.MetodoPago>
{
  private readonly IMetodoPagoService _metodoPagoService;
  private readonly IAuthorizationService _authorizationService;

  public GetMetodoPagoByIdEndpoint(IMetodoPagoService metodoPagoService, IAuthorizationService authorizationService)
  {
    _metodoPagoService = metodoPagoService;
    _authorizationService = authorizationService;
  }

  public override void Configure()
  {
    Verbs(Http.GET);
    Routes("/metodo-pago/{MetodoPagoId:guid}");
    Summary(s =>
    {
      s.Summary = "Obtener método de pago por ID";
      s.Description = "Obtiene un método de pago de la base de datos por su ID";
      s.ExampleRequest = new GetMetodoPagoByIdRequest
      {
        MetodoPagoId = Guid.NewGuid()
      };
      s.ResponseExamples[200] = new Domain.Entities.MetodoPago
      {
        IdMetodoPago = Guid.NewGuid(),
        TipoEntidad = "Cliente",
        IdEntidad = Guid.NewGuid(),
        Proveedor = "Proveedor Ejemplo",
        FechaExpiracion = DateTime.UtcNow.AddYears(1),
        Activo = true,
        FechaRegistro = DateTime.UtcNow
      };
      s.Responses[404] = "Método de pago no encontrado";
      s.Responses[200] = "Método de pago encontrado";
    });
  }

  public override async Task HandleAsync(GetMetodoPagoByIdRequest req, CancellationToken ct)
  {
    var roleGuids = User.Claims.Where(c => c.Type == "role").Select(c => Guid.Parse(c.Value)).ToArray();

    if (!await _authorizationService.IsRoleAuthorizedToEndpointAsync(roleGuids, "Ver_Metodo_Pago"))
    {
      await SendUnauthorizedAsync(ct);
    }

    var metodoPago = await _metodoPagoService.GetByIdAsync(req.MetodoPagoId);
    if (metodoPago == null)
    {
      await SendNotFoundAsync(ct);
      return;
    }

    await SendOkAsync(metodoPago, ct);
  }
}