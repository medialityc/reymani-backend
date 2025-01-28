namespace reymani_web_api.Api.Endpoints.NegocioUsuario
{
  public sealed class DeleteNegocioUsuarioEndpoint : Endpoint<DeleteNegocioUsuarioRequest>
  {
    private readonly INegocioUsuarioService _negocioUsuarioService;
    private readonly IAuthorizationService _authorizationService;
    private readonly INegocioService _negocioService;
    private readonly IUsuarioService _UsuarioService;

    public DeleteNegocioUsuarioEndpoint(INegocioUsuarioService negocioUsuarioService, IAuthorizationService authorizationService, INegocioService negocioService, IUsuarioService UsuarioService)
    {
      _negocioUsuarioService = negocioUsuarioService;
      _authorizationService = authorizationService;
      _UsuarioService = UsuarioService;
      _negocioService = negocioService;
    }

    public override void Configure()
    {
      Verbs(Http.DELETE);
      Routes("/negocio-usuario/{UsuarioId}/{NegocioId}");
      Summary(s =>
      {
        s.Summary = "Eliminar NegocioUsuario";
        s.Description = "Elimina un Negocio-Usuario. Es decir, se elimina la suscripcion del Usuario a dicho negocio.";
        s.ExampleRequest = new DeleteNegocioUsuarioRequest
        {
          UsuarioId = Guid.NewGuid(),
          NegocioId = Guid.NewGuid()
        };
      });
    }

    public override async Task HandleAsync(DeleteNegocioUsuarioRequest req, CancellationToken ct)
    {
      var roleGuids = User.Claims.Where(c => c.Type == "role").Select(c => Guid.Parse(c.Value)).ToArray();

      if (!await _authorizationService.IsRoleAuthorizedToEndpointAsync(roleGuids, "Eliminar_Negocio_Usuario"))
      {
        await SendUnauthorizedAsync(ct);
        return;
      }

      var negocio = await _negocioService.GetByIdAsync(req.NegocioId);
      if (negocio == null)
      {
        AddError(req => req.NegocioId, "Negocio no encontrado");
      }

      var Usuario = await _UsuarioService.GetUsuarioByIdAsync(req.UsuarioId);
      if (Usuario == null)
      {
        AddError(req => req.UsuarioId, "Usuario no encontrado");
      }

      var negocioUsuario = await _negocioUsuarioService.GetByIdUsuarioAndIdNegocio(req.UsuarioId, req.NegocioId);
      if (negocioUsuario == null)
      {
        AddError("El Usuario no se encuentra subscrito al negocio.");
      }

      ThrowIfAnyErrors();

      await _negocioUsuarioService.DeleteAsync(req.UsuarioId, req.NegocioId);
      await SendOkAsync(ct);
    }
  }
}
