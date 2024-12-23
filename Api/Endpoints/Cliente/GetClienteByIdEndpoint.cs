using System;
using reymani_web_api.Application.DTOs;
using reymani_web_api.Application.Services;

namespace reymani_web_api.Api.Endpoints.Cliente
{
  public class GetClienteByIdEndpoint : Endpoint<GetClienteByIdRequest, ClienteDto>
  {
    private readonly IClienteService _clienteService;

    public GetClienteByIdEndpoint(IClienteService clienteService)
    {
      _clienteService = clienteService;
    }

    public override void Configure()
    {
      Verbs(Http.GET);
      Routes("/cliente/{ClienteId:guid}");
      Summary(s =>
      {
        s.Summary = "Obtener cliente por ID";
        s.Description = "Obtiene un cliente de la base de datos por su ID";
        s.ExampleRequest = new GetClienteByIdRequest
        {
          ClienteId = Guid.NewGuid()
        };
        s.ResponseExamples[200] = new ClienteDto
        {
          Id = Guid.NewGuid(),
          NumeroCarnet = "12345678",
          Nombre = "John",
          Apellidos = "Doe",
          Username = "johndoe"
        };
        s.Responses[404] = "Cliente no encontrado";
        s.Responses[200] = "Cliente encontrado";
      });
    }

    public override async Task HandleAsync(GetClienteByIdRequest req, CancellationToken ct)
    {
      var cliente = await _clienteService.GetClienteByIdAsync(req.ClienteId);
      if (cliente == null)
      {
        await SendNotFoundAsync(ct);
        return;
      }

      var clienteDto = new ClienteDto
      {
        Id = cliente.IdCliente,
        NumeroCarnet = cliente.NumeroCarnet,
        Nombre = cliente.Nombre,
        Apellidos = cliente.Apellidos,
        Username = cliente.Username
      };

      await SendOkAsync(clienteDto, ct);
    }
  }
}
