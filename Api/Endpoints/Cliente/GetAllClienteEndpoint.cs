using System;
using reymani_web_api.Application.DTOs;

namespace reymani_web_api.Api.Endpoints.Cliente;

public class GetAllClienteEndpoint : EndpointWithoutRequest<GetAllClienteResponse>
{
  private readonly IClienteService _clienteService;
  private readonly IAuthorizationService _authorizationService;

  public GetAllClienteEndpoint(IClienteService clienteService, IAuthorizationService authorizationService)
  {
    _clienteService = clienteService;
    _authorizationService = authorizationService;
  }

  public override void Configure()
  {
    Get("/cliente");
    Summary(s =>
    {
      s.Summary = "Obtener todos los clientes";
      s.Description = "Obtiene todos los clientes de la base de datos";
      s.ResponseExamples[200] = new GetAllClienteResponse
      {
        Clientes = new List<ClienteDto>
        {
          new ClienteDto
          {
            Id = Guid.NewGuid(),
            NumeroCarnet = "12345678",
            Nombre = "John",
            Apellidos = "Doe",
            Username = "johndoe"
          }
        }
      };
    });
  }

  public override async Task<GetAllClienteResponse> ExecuteAsync(CancellationToken ct)
  {
    var roleGuids = User.Claims.Where(c => c.Type == "role").Select(c => Guid.Parse(c.Value)).ToArray();

    if (!await _authorizationService.IsRoleAuthorizedToEndpointAsync(roleGuids, "Ver_Clientes"))
    {
      await SendUnauthorizedAsync(ct);
    }

    var clientes = await _clienteService.GetAllClientesAsync();
    return new GetAllClienteResponse
    {
      Clientes = clientes.Select(c => new ClienteDto
      {
        Id = c.IdCliente,
        NumeroCarnet = c.NumeroCarnet,
        Nombre = c.Nombre,
        Apellidos = c.Apellidos,
        Username = c.Username
      }).ToList()
    };
  }
}