using System;

namespace reymani_web_api.Api.Endpoints.Cliente;

public sealed class ChangeClientePasswordEndpoint : Endpoint<ChangeClientePasswordRequest>
{
  private readonly IClienteService _clienteService;

  public ChangeClientePasswordEndpoint(IClienteService clienteService)
  {
    _clienteService = clienteService;
  }

  public override void Configure()
  {
    Verbs(Http.PUT);
    Routes("/cliente/change-password");
  }

  public override async Task HandleAsync(ChangeClientePasswordRequest req, CancellationToken ct)
  {
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