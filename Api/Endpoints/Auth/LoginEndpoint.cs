namespace reymani_web_api.Api.Endpoints.Auth;

public class LoginEndpoint : Endpoint<LoginRequest, LoginResponse>
{
  private readonly IAuthService _authService;
  private readonly IUsuarioService _UsuarioService;

  public LoginEndpoint(IAuthService authService, IUsuarioService UsuarioService)
  {
    _authService = authService;
    _UsuarioService = UsuarioService;
  }

  public override void Configure()
  {
    Verbs(Http.POST);
    Routes("/auth/login");
    AllowAnonymous();
    Summary(s =>
    {
      s.Summary = "Iniciar sesión";
      s.Description = "Inicia sesión en la aplicación";
      s.ExampleRequest = new LoginRequest
      {
        UsernameOrPhone = "rafael",
        Password = "Rafael123"
      };
      s.ResponseExamples[200] = new LoginResponse
      {
        Token = "fhusdyr723ryui23rh7891y43u1b4u12gbrfef13",
        Permissions = new List<string>
        {
          "Ver_Usuarios",
          "Crear_Usuarios",
          "Editar_Usuarios",
          "Eliminar_Usuarios"
        }
      };
    });
  }

  public override async Task HandleAsync(LoginRequest req, CancellationToken ct)
  {
    try
    {
      var token = await _authService.LoginAsync(req);
      var UsuarioId = _authService.GetIdUsuarioFromToken(token);
      var permissions = await _UsuarioService.GetPermissionsAsync(UsuarioId);
      await SendOkAsync(new LoginResponse { Token = token, Permissions = permissions }, ct);
    }
    catch (UnauthorizedAccessException)
    {
      AddError("Credenciales inválidas");
      ThrowIfAnyErrors();
    }
  }
}
