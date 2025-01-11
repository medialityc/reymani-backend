
namespace reymani_web_api.Api.Endpoints.MetodoPago;

public class GetAllMetodoPagoEndpoint : EndpointWithoutRequest<GetAllMetodoPagoResponse>
{
  private readonly IMetodoPagoService _metodoPagoService;
  private readonly IAuthorizationService _authorizationService;

  public GetAllMetodoPagoEndpoint(IMetodoPagoService metodoPagoService, IAuthorizationService authorizationService)
  {
    _metodoPagoService = metodoPagoService;
    _authorizationService = authorizationService;
  }

  public override void Configure()
  {
    Get("/metodo-pago");
    Summary(s =>
    {
      s.Summary = "Obtener todos los métodos de pago";
      s.Description = "Obtiene todos los métodos de pago de la base de datos";
      s.ResponseExamples[200] = new GetAllMetodoPagoResponse
      {
        MetodosPago = new List<Domain.Entities.MetodoPago>
        {
          new Domain.Entities.MetodoPago
          {
            IdMetodoPago = Guid.NewGuid(),
            TipoEntidad = "Cliente",
            IdEntidad = Guid.NewGuid(),
            Proveedor = "Proveedor Ejemplo",
            FechaExpiracion = DateTime.UtcNow.AddYears(1),
            Activo = true,
            FechaRegistro = DateTime.UtcNow
          }
        }
      };
    });
  }

  public override async Task<GetAllMetodoPagoResponse> ExecuteAsync(CancellationToken ct)
  {
    var roleGuids = User.Claims.Where(c => c.Type == "role").Select(c => Guid.Parse(c.Value)).ToArray();

    if (!await _authorizationService.IsRoleAuthorizedToEndpointAsync(roleGuids, "Ver_Metodos_Pago"))
    {
      await SendUnauthorizedAsync(ct);
    }

    var metodosPago = await _metodoPagoService.GetAllAsync();
    return new GetAllMetodoPagoResponse
    {
      MetodosPago = metodosPago.ToList()
    };
  }
}