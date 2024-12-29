using System;
using YamlDotNet.Core.Tokens;

namespace reymani_web_api.Api.Endpoints.Auth;

public class LoginEndpoint : Endpoint<LoginRequest, LoginResponse>
{
  private readonly IAuthService _authService;
  private readonly IClienteService _clienteService;

  public LoginEndpoint(IAuthService authService, IClienteService clienteService)
  {
    _authService = authService;
    _clienteService = clienteService;
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
        UsernameOrPhone = "johndoe",
        Password = "Jhondoe123"
      };
      s.ResponseExamples[200] = new LoginResponse
      {
        Token = "fhusdyr723ryui23rh7891y43u1b4u12gbrfef13",
        Permissions = new List<string>
        {
          "Ver_Clientes",
          "Crear_Clientes",
          "Editar_Clientes",
          "Eliminar_Clientes"
        }
      };
    });
  }

  public override async Task HandleAsync(LoginRequest req, CancellationToken ct)
  {
    try
    {
      var token = await _authService.LoginAsync(req);
      var clienteId = await _authService.GetIdClienteFromTokenAsync(token);
      var permissions = await _clienteService.GetPermissionsAsync(clienteId);
      await SendOkAsync(new LoginResponse { Token = token, Permissions = permissions }, ct);
    }
    catch (UnauthorizedAccessException)
    {
      AddError("Credenciales inválidas");
      ThrowIfAnyErrors();
    }
  }
}
