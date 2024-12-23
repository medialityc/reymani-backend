using System;

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
    Routes("/cliente/update");
    Summary(s =>
    {
      s.Summary = "Actualizar Cliente";
      s.Description = "Actualiza un cliente en la base de datos";
      s.ExampleRequest = new UpdateClienteRequest
      {
        IdCliente = Guid.NewGuid(),
        NumeroCarnet = "12345678",
        Nombre = "John",
        Apellidos = "Doe",
        Username = "johndoe",
        Password = "Jhondoe123"
      };
    });
  }

  public override async Task HandleAsync(UpdateClienteRequest req, CancellationToken ct)
  {
    try
    {
      await _clienteService.UpdateClienteAsync(req);
      await SendOkAsync();
    }
    catch (Exception e)
    {
      AddError(e.Message);
      ThrowIfAnyErrors();
    }
  }
}