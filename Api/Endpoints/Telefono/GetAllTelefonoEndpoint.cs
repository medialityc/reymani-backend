namespace reymani_web_api.Api.Endpoints.Telefono;

public class GetAllTelefonoEndpoint : EndpointWithoutRequest<GetAllTelefonoResponse>
{
  private readonly ITelefonoService _telefonoService;
  private readonly IAuthorizationService _authorizationService;

  public GetAllTelefonoEndpoint(ITelefonoService telefonoService, IAuthorizationService authorizationService)
  {
    _telefonoService = telefonoService;
    _authorizationService = authorizationService;
  }

  public override void Configure()
  {
    Get("/telefono");
    Summary(s =>
    {
      s.Summary = "Obtener todos los teléfonos";
      s.Description = "Obtiene todos los teléfonos de la base de datos";
      s.ResponseExamples[200] = new GetAllTelefonoResponse
      {
        Telefonos = new List<Domain.Entities.Telefono>
        {
          new Domain.Entities.Telefono
          {
            IdTelefono = Guid.NewGuid(),
            TipoEntidad = "Cliente",
            IdEntidad = Guid.NewGuid(),
            NumeroTelefono = "123456789",
            Descripcion = "Teléfono de ejemplo"
          }
        }
      };
    });
  }

  public override async Task<GetAllTelefonoResponse> ExecuteAsync(CancellationToken ct)
  {
    var roleGuids = User.Claims.Where(c => c.Type == "role").Select(c => Guid.Parse(c.Value)).ToArray();

    if (!await _authorizationService.IsRoleAuthorizedToEndpointAsync(roleGuids, "Ver_Telefonos"))
    {
      await SendUnauthorizedAsync(ct);
    }

    var telefonos = await _telefonoService.GetAllAsync();
    return new GetAllTelefonoResponse
    {
      Telefonos = telefonos.ToList()
    };
  }
}
