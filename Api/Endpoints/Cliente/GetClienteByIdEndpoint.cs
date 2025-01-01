using System;
using reymani_web_api.Application.DTOs;
using reymani_web_api.Application.Services;

namespace reymani_web_api.Api.Endpoints.Cliente
{
  public class GetClienteByIdEndpoint : Endpoint<GetClienteByIdRequest, ClienteDto>
  {
    private readonly IClienteService _clienteService;
    private readonly IAuthorizationService _authorizationService;

    public GetClienteByIdEndpoint(IClienteService clienteService, IAuthorizationService authorizationService)
    {
      _clienteService = clienteService;
      _authorizationService = authorizationService;
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
          Username = "johndoe",
          Activo = true
        };
        s.Responses[404] = "Cliente no encontrado";
        s.Responses[200] = "Cliente encontrado";
      });
    }

    public override async Task HandleAsync(GetClienteByIdRequest req, CancellationToken ct)
    {
      var roleGuids = User.Claims.Where(c => c.Type == "role").Select(c => Guid.Parse(c.Value)).ToArray();

      if (!await _authorizationService.IsRoleAuthorizedToEndpointAsync(roleGuids, "Ver_Cliente"))
      {
        await SendUnauthorizedAsync(ct);
      }

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
        Username = cliente.Username,
        Activo = cliente.Activo
      };

      await SendOkAsync(clienteDto, ct);
    }
  }
}
