using System;

namespace reymani_web_api.Api.Endpoints.Cliente;

public sealed class ChangeClientePasswordEndpoint : Endpoint<ChangeClientePasswordRequest>
{
  private readonly IClienteService _clienteService;
  private readonly IAuthorizationService _authorizationService;

  public ChangeClientePasswordEndpoint(IClienteService clienteService, IAuthorizationService authorizationService)
  {
    _clienteService = clienteService;
    _authorizationService = authorizationService;
  }

  public override void Configure()
  {
    Verbs(Http.PUT);
    Routes("/cliente/change-password");
    Summary(s =>
      {
        s.Summary = "Cambiar contraseña del Cliente";
        s.Description = "Cambiar contraseña del Cliente en la base de datos";
        s.ExampleRequest = new ChangeClientePasswordRequest
        {
          ClienteId = new Guid(),
          NewPassword = "dasdsadsa",
          Password = "sdadsadsadsadsa"
        };
      });
  }

  public override async Task HandleAsync(ChangeClientePasswordRequest req, CancellationToken ct)
  {
    var roleGuids = User.Claims.Where(c => c.Type == "role").Select(c => Guid.Parse(c.Value)).ToArray();

    if (!await _authorizationService.IsRoleAuthorizedToEndpointAsync(roleGuids, "Cambiar_Contraseña"))
    {
      await SendUnauthorizedAsync(ct);
    }

    var cliente = await _clienteService.GetClienteByIdAsync(req.ClienteId);
    if (cliente == null)
    {
      await SendNotFoundAsync(ct);
      return;
    }

    if (!await _clienteService.CheckPasswordAsync(cliente, req.Password))
    {
      AddError(r => r.Password, "Contraseña incorrecta");
    }

    ThrowIfAnyErrors();

    await _clienteService.ChangePasswordAsync(cliente, req.NewPassword);
    await SendOkAsync(ct);
  }
}