namespace reymani_web_api.Api.Endpoints.Negocio;

public sealed class CreateNegocioEndpoint : Endpoint<CreateNegocioRequest>
{
  private readonly INegocioService _negocioService;
  private readonly IAuthorizationService _authorizationService;

  public CreateNegocioEndpoint(INegocioService negocioService, IAuthorizationService authorizationService)
  {
    _negocioService = negocioService;
    _authorizationService = authorizationService;
  }

  public override void Configure()
  {
    Verbs(Http.POST);
    Routes("/negocio");
    Summary(s =>
    {
      s.Summary = "Crear Negocio";
      s.Description = "Crea un nuevo negocio";
      s.ExampleRequest = new CreateNegocioRequest
      {
        Nombre = "Negocio de Prueba",
        Descripcion = "Descripción del negocio de prueba",
        EntregaDomicilio = true,
        URLImagenPrincipal = "http://example.com/imagen.jpg",
        URLImagenLogo = "http://example.com/logo.jpg",
        URLImagenBanner = "http://example.com/banner.jpg",
      };
    });
  }

  public override async Task HandleAsync(CreateNegocioRequest req, CancellationToken ct)
  {
    var roleGuids = User.Claims.Where(c => c.Type == "role").Select(c => Guid.Parse(c.Value)).ToArray();

    if (!await _authorizationService.IsRoleAuthorizedToEndpointAsync(roleGuids, "Crear_Negocio"))
    {
      await SendUnauthorizedAsync(ct);
    }

    var negocio = new reymani_web_api.Domain.Entities.Negocio
    {
      IdNegocio = Guid.NewGuid(),
      Nombre = req.Nombre,
      Descripcion = req.Descripcion,
      EntregaDomicilio = req.EntregaDomicilio,
      URLImagenPrincipal = req.URLImagenPrincipal,
      URLImagenLogo = req.URLImagenLogo,
      URLImagenBanner = req.URLImagenBanner
    };

    await _negocioService.AddAsync(negocio);

    await SendOkAsync(negocio, ct);
  }
}
