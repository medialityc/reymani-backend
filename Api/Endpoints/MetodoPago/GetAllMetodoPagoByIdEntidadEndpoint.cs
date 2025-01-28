
namespace reymani_web_api.Api.Endpoints.MetodoPago;

public class GetAllMetodoPagoByIdEntidadEndpoint : Endpoint<GetAllMetodoPagoByIdEntidadRequest, IEnumerable<Domain.Entities.MetodoPago>>
{
  private readonly IMetodoPagoService _metodoPagoService;
  private readonly IAuthorizationService _authorizationService;

  public GetAllMetodoPagoByIdEntidadEndpoint(IMetodoPagoService metodoPagoService, IAuthorizationService authorizationService)
  {
    _metodoPagoService = metodoPagoService;
    _authorizationService = authorizationService;
  }

  public override void Configure()
  {
    Verbs(Http.GET);
    Routes("/metodo-pago/entidad/{IdEntidad:guid}");
    Summary(s =>
    {
      s.Summary = "Obtener todos los métodos de pago por ID de entidad";
      s.Description = "Obtiene todos los métodos de pago de la base de datos por el ID de la entidad";
      s.ExampleRequest = new GetAllMetodoPagoByIdEntidadRequest
      {
        IdEntidad = Guid.NewGuid()
      };
      s.ResponseExamples[200] = new List<Domain.Entities.MetodoPago>
      {
        new Domain.Entities.MetodoPago
        {
          IdMetodoPago = Guid.NewGuid(),
          TipoEntidad = "Usuario",
          IdEntidad = Guid.NewGuid(),
          Proveedor = "Proveedor Ejemplo",
          FechaExpiracion = DateTime.UtcNow.AddYears(1),
          Activo = true,
          FechaRegistro = DateTime.UtcNow
        }
      };
      s.Responses[404] = "Métodos de pago no encontrados";
      s.Responses[200] = "Métodos de pago encontrados";
    });
  }

  public override async Task HandleAsync(GetAllMetodoPagoByIdEntidadRequest req, CancellationToken ct)
  {
    var roleGuids = User.Claims.Where(c => c.Type == "role").Select(c => Guid.Parse(c.Value)).ToArray();

    if (!await _authorizationService.IsRoleAuthorizedToEndpointAsync(roleGuids, "Ver_Metodos_Pago_Entidad"))
    {
      await SendUnauthorizedAsync(ct);
    }

    var metodosPago = await _metodoPagoService.GetAllByIdEntidadAsync(req.IdEntidad);
    if (metodosPago == null || !metodosPago.Any())
    {
      await SendNotFoundAsync(ct);
      return;
    }

    await SendOkAsync(metodosPago, ct);
  }
}