namespace reymani_web_api.Api.Endpoints.CategoriaNegocio;

public sealed class CreateCategoriaNegocioEndpoint : Endpoint<CreateCategoriaNegocioRequest>
{
  private readonly ICategoriaNegocioService _categoriaNegocioService;
  private readonly IAuthorizationService _authorizationService;

  public CreateCategoriaNegocioEndpoint(ICategoriaNegocioService categoriaNegocioService, IAuthorizationService authorizationService)
  {
    _categoriaNegocioService = categoriaNegocioService;
    _authorizationService = authorizationService;
  }

  public override void Configure()
  {
    Verbs(Http.POST);
    Routes("/categoria-negocio");
    Summary(s =>
    {
      s.Summary = "Crear Categoría de Negocio";
      s.Description = "Crea una nueva categoría de negocio";
      s.ExampleRequest = new CreateCategoriaNegocioRequest
      {
        Nombre = "Restaurantes",
        Descripcion = "Categoría para restaurantes"
      };
    });
  }

  public override async Task HandleAsync(CreateCategoriaNegocioRequest req, CancellationToken ct)
  {
    var roleGuids = User.Claims.Where(c => c.Type == "role").Select(c => Guid.Parse(c.Value)).ToArray();

    if (!await _authorizationService.IsRoleAuthorizedToEndpointAsync(roleGuids, "Crear_Categoria_Negocio"))
    {
      await SendUnauthorizedAsync(ct);
    }

    var existingCategoria = await _categoriaNegocioService.GetByNameAsync(req.Nombre);
    if (existingCategoria != null)
    {
      AddError(r => r.Nombre, "El nombre de la categoría ya existe");
      ThrowIfAnyErrors();
    }

    var categoriaNegocio = new Domain.Entities.CategoriaNegocio
    {
      IdCategoria = Guid.NewGuid(),
      Nombre = req.Nombre,
      Descripcion = req.Descripcion
    };

    await _categoriaNegocioService.AddAsync(categoriaNegocio);

    await SendOkAsync(categoriaNegocio, ct);
  }
}
