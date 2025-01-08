namespace reymani_web_api.Api.Endpoints.CategoriaNegocio;

public sealed class DeleteCategoriaNegocioEndpoint : Endpoint<DeleteCategoriaNegocioRequest>
{
  private readonly ICategoriaNegocioService _categoriaNegocioService;
  private readonly IAuthorizationService _authorizationService;

  public DeleteCategoriaNegocioEndpoint(ICategoriaNegocioService categoriaNegocioService, IAuthorizationService authorizationService)
  {
    _categoriaNegocioService = categoriaNegocioService;
    _authorizationService = authorizationService;
  }

  public override void Configure()
  {
    Verbs(Http.DELETE);
    Routes("/categoria-negocio/{CategoriaNegocioId}");
    Summary(s =>
    {
      s.Summary = "Eliminar Categoría de Negocio";
      s.Description = "Elimina una Categoría de Negocio";
      s.ExampleRequest = new DeleteCategoriaNegocioRequest
      {
        CategoriaNegocioId = Guid.NewGuid()
      };
    });
  }

  public override async Task HandleAsync(DeleteCategoriaNegocioRequest req, CancellationToken ct)
  {
    var roleGuids = User.Claims.Where(c => c.Type == "role").Select(c => Guid.Parse(c.Value)).ToArray();

    if (!await _authorizationService.IsRoleAuthorizedToEndpointAsync(roleGuids, "Eliminar_Categoria_Negocio"))
    {
      await SendUnauthorizedAsync(ct);
    }

    var categoriaNegocio = await _categoriaNegocioService.GetByIdAsync(req.CategoriaNegocioId);
    if (categoriaNegocio == null)
    {
      await SendNotFoundAsync(ct);
      return;
    }

    await _categoriaNegocioService.DeleteAsync(req.CategoriaNegocioId);
    await SendOkAsync(ct);
  }
}
