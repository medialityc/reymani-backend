using System;

namespace reymani_web_api.Api.Endpoints.Auth;

public class RegisterEndpoint : Endpoint<RegisterRequest, RegisterResponse>
{
  private readonly IAuthService _authService;
  private readonly IClienteService _clienteService;

  public RegisterEndpoint(IAuthService authService, IClienteService clienteService)
  {
    _authService = authService;
    _clienteService = clienteService;
  }

  public override void Configure()
  {
    Verbs(Http.POST);
    Routes("/auth/register");
    AllowAnonymous();
    Summary(s =>
    {
      s.Summary = "Registrar Cliente";
      s.Description = "Registra un nuevo cliente en la aplicación";
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
          "Ver_Clientes",
          "Crear_Clientes",
          "Editar_Clientes",
          "Eliminar_Clientes"
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
    var clienteId = await _authService.GetIdClienteFromTokenAsync(token);
    var permissions = await _clienteService.GetPermissionsAsync(clienteId);
    await SendOkAsync(new RegisterResponse { Token = token, Permissions = permissions }, ct);
  }
}
