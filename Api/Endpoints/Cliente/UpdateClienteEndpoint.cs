using System;
using reymani_web_api.Application.DTOs;

namespace reymani_web_api.Api.Endpoints.Cliente;

public class UpdateClienteEndpoint : Endpoint<UpdateClienteRequest>
{
  private readonly IClienteService _clienteService;

  public UpdateClienteEndpoint(IClienteService clienteService)
  {
    _clienteService = clienteService;
  }

  public override void Configure()
  {
    Verbs(Http.PUT);
    Routes("/cliente/{IdCliente:guid}");
    Summary(s =>
    {
      s.Summary = "Actualizar Cliente";
      s.Description = "Actualiza un cliente en la base de datos";
      s.ExampleRequest = new UpdateClienteRequest
      {
        IdCliente = Guid.NewGuid(),
        Cliente = new ClienteDto
        {
          NumeroCarnet = "04022067256",
          Nombre = "John",
          Apellidos = "Doe",
          Username = "johndoe"
        }
      };
    });

  }

  public override async Task HandleAsync(UpdateClienteRequest req, CancellationToken ct)
  {
    var cliente = await _clienteService.GetClienteByIdAsync(req.IdCliente);
    if (cliente == null)
    {
      await SendNotFoundAsync(ct);
      return;
    }

    if (req.Cliente.Id != req.IdCliente)
    {
      AddError(r => r.IdCliente, "El ID del cliente no coincide con el ID de la URL");
    }

    ThrowIfAnyErrors();

    if (cliente != null)
    {
      cliente.NumeroCarnet = req.Cliente.NumeroCarnet;
      cliente.Nombre = req.Cliente.Nombre;
      cliente.Apellidos = req.Cliente.Apellidos;
      cliente.Username = req.Cliente.Username;
      await _clienteService.UpdateClienteAsync(cliente);
      await SendOkAsync(ct);
    }
  }
}