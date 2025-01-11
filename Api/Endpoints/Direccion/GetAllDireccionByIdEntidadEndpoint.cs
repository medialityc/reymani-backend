
namespace reymani_web_api.Api.Endpoints.Direccion
{
  public class GetAllDireccionByIdEntidadEndpoint : Endpoint<GetAllDireccionByIdEntidadRequest, IEnumerable<Domain.Entities.Direccion>>
  {
    private readonly IDireccionService _direccionService;
    private readonly IAuthorizationService _authorizationService;

    public GetAllDireccionByIdEntidadEndpoint(IDireccionService direccionService, IAuthorizationService authorizationService)
    {
      _direccionService = direccionService;
      _authorizationService = authorizationService;
    }

    public override void Configure()
    {
      Verbs(Http.GET);
      Routes("/direccion/entidad/{IdEntidad:guid}");
      Summary(s =>
      {
        s.Summary = "Obtener todas las direcciones por ID de entidad";
        s.Description = "Obtiene todas las direcciones de la base de datos por el ID de la entidad";
        s.ExampleRequest = new GetAllDireccionByIdEntidadRequest
        {
          IdEntidad = Guid.NewGuid()
        };
        s.ResponseExamples[200] = new List<Domain.Entities.Direccion>
        {
          new Domain.Entities.Direccion
          {
            IdDireccion = Guid.NewGuid(),
            TipoEntidad = "Cliente",
            IdEntidad = Guid.NewGuid(),
            DireccionEntidad = "123 Calle Principal",
            Municipio = "Ciudad",
            Provincia = "Provincia",
            Latitud = 0.0,
            Longitud = 0.0,
            Descripcion = "Dirección de ejemplo"
          }
        };
        s.Responses[404] = "Direcciones no encontradas";
        s.Responses[200] = "Direcciones encontradas";
      });
    }

    public override async Task HandleAsync(GetAllDireccionByIdEntidadRequest req, CancellationToken ct)
    {
      var roleGuids = User.Claims.Where(c => c.Type == "role").Select(c => Guid.Parse(c.Value)).ToArray();

      if (!await _authorizationService.IsRoleAuthorizedToEndpointAsync(roleGuids, "Ver_Direcciones_Entidad"))
      {
        await SendUnauthorizedAsync(ct);
      }

      var direcciones = await _direccionService.GetAllByIdEntidadAsync(req.IdEntidad);
      if (direcciones == null || !direcciones.Any())
      {
        await SendNotFoundAsync(ct);
        return;
      }

      await SendOkAsync(direcciones, ct);
    }
  }
}
