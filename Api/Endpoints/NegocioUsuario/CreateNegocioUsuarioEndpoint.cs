namespace reymani_web_api.Api.Endpoints.NegocioUsuario;

public sealed class CreateNegocioUsuarioEndpoint : Endpoint<CreateNegocioUsuarioRequest>
{
  private readonly INegocioUsuarioService _negocioUsuarioService;
  private readonly IAuthorizationService _authorizationService;
  private readonly INegocioService _negocioService;
  private readonly IUsuarioService _UsuarioService;

  public CreateNegocioUsuarioEndpoint(INegocioUsuarioService negocioUsuarioService, IAuthorizationService authorizationService, INegocioService negocioService, IUsuarioService UsuarioService)
  {
    _negocioUsuarioService = negocioUsuarioService;
    _authorizationService = authorizationService;
    _negocioService = negocioService;
    _UsuarioService = UsuarioService;
  }

  public override void Configure()
  {
    Verbs(Http.POST);
    Routes("/negocio-usuario");
    Summary(s =>
    {
      s.Summary = "Crear Negocio-Usuario";
      s.Description = "Crea una nueva relación entre negocio y Usuario. Es decir, subscribir Usuario a un negocio dado.";
      s.ExampleRequest = new CreateNegocioUsuarioRequest
      {
        IdUsuario = Guid.NewGuid(),
        IdNegocio = Guid.NewGuid()
      };
    });
  }

  public override async Task HandleAsync(CreateNegocioUsuarioRequest req, CancellationToken ct)
  {
    var roleGuids = User.Claims.Where(c => c.Type == "role").Select(c => Guid.Parse(c.Value)).ToArray();

    if (!await _authorizationService.IsRoleAuthorizedToEndpointAsync(roleGuids, "Crear_Negocio_Usuario"))
    {
      await SendUnauthorizedAsync(ct);
    }

    var negocio = await _negocioService.GetByIdAsync(req.IdNegocio);
    if (negocio == null)
    {
      AddError(req => req.IdNegocio, "Negocio no encontrado");
    }

    var Usuario = await _UsuarioService.GetUsuarioByIdAsync(req.IdUsuario);
    if (Usuario == null)
    {
      AddError(req => req.IdUsuario, "Usuario no encontrado");
    }

    var negocioUsuario = new Domain.Entities.NegocioUsuario
    {
      IdNegocioUsuario = Guid.NewGuid(),
      IdUsuario = req.IdUsuario,
      IdNegocio = req.IdNegocio
    };

    await _negocioUsuarioService.AddAsync(negocioUsuario);

    await SendOkAsync(negocioUsuario, ct);
  }
}
