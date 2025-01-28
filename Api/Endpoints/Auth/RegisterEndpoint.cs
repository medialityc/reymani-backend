using System;

namespace reymani_web_api.Api.Endpoints.Auth;

public class RegisterEndpoint : Endpoint<RegisterRequest, RegisterResponse>
{
  private readonly IAuthService _authService;
  private readonly IUsuarioService _UsuarioService;

  public RegisterEndpoint(IAuthService authService, IUsuarioService UsuarioService)
  {
    _authService = authService;
    _UsuarioService = UsuarioService;
  }

  public override void Configure()
  {
    Verbs(Http.POST);
    Routes("/auth/register");
    AllowAnonymous();
    Summary(s =>
    {
      s.Summary = "Registrar Usuario";
      s.Description = "Registra un nuevo Usuario en la aplicación";
      s.ExampleRequest = new RegisterRequest
      {
        NumeroCarnet = "04112086258",
        Nombre = "John",
        Apellidos = "Doe Martin",
        Username = "johndoe",
        Password = "Jhondoe123"
      };
      s.ResponseExamples[200] = new RegisterResponse
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

  public override async Task HandleAsync(RegisterRequest req, CancellationToken ct)
  {
    if (await _authService.IsNumeroCarnetInUseAsync(req.NumeroCarnet))
    {
      AddError("El número de carnet ya está en uso");
    }

    if (await _authService.IsUsernameInUseAsync(req.Username))
    {
      AddError("El nombre de usuario ya está en uso");
    }

    ThrowIfAnyErrors();
    var token = await _authService.RegisterAsync(req);
    var UsuarioId = _authService.GetIdUsuarioFromToken(token);
    var permissions = await _UsuarioService.GetPermissionsAsync(UsuarioId);
    await SendOkAsync(new RegisterResponse { Token = token, Permissions = permissions }, ct);
  }
}
