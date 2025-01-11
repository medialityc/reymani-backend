
using reymani_web_api.Application.DTOs;

namespace reymani_web_api.Api.Endpoints.Negocio
{
  public class GetClientesByIdNegocioEndpoint : Endpoint<GetClientesByIdNegocioRequest, GetClientesByIdNegocioResponse>
  {
    private readonly INegocioClienteService _negocioClienteService;
    private readonly IAuthorizationService _authorizationService;
    private readonly INegocioService _negocioService;

    public GetClientesByIdNegocioEndpoint(INegocioClienteService negocioClienteService, IAuthorizationService authorizationService, INegocioService negocioService)
    {
      _negocioClienteService = negocioClienteService;
      _authorizationService = authorizationService;
      _negocioService = negocioService;
    }

    public override void Configure()
    {
      Verbs(Http.GET);
      Routes("/negocio/{IdNegocio:guid}/clientes");
      Summary(s =>
      {
        s.Summary = "Obtener todos los clientes por ID de negocio";
        s.Description = "Obtiene todos los clientes de la base de datos por el ID del negocio";
        s.ExampleRequest = new GetClientesByIdNegocioRequest
        {
          IdNegocio = Guid.NewGuid()
        };
        s.ResponseExamples[200] = new GetClientesByIdNegocioResponse
        {
          Clientes = new List<ClienteDto>
          {
            new ClienteDto
            {
              Id = Guid.NewGuid(),
              NumeroCarnet = "12345678901",
              Nombre = "Cliente Ejemplo",
              Apellidos = "Apellido Ejemplo",
              Username = "clienteejemplo",
              Activo = true
            }
          }
        };
        s.Responses[404] = "Clientes no encontrados";
        s.Responses[200] = "Clientes encontrados";
      });
    }

    public override async Task HandleAsync(GetClientesByIdNegocioRequest req, CancellationToken ct)
    {
      var roleGuids = User.Claims.Where(c => c.Type == "role").Select(c => Guid.Parse(c.Value)).ToArray();

      if (!await _authorizationService.IsRoleAuthorizedToEndpointAsync(roleGuids, "Ver_Clientes_Del_Negocio"))
      {
        await SendUnauthorizedAsync(ct);
      }

      var negocio = await _negocioService.GetByIdAsync(req.IdNegocio);
      if (negocio == null)
      {
        AddError(req => req.IdNegocio, "Negocio no encontrado");
      }

      ThrowIfAnyErrors();

      var clientes = await _negocioClienteService.GetClientesByNegocioIdAsync(req.IdNegocio);
      var clienteDtos = clientes.Select(c => new ClienteDto
      {
        Id = c.IdCliente,
        NumeroCarnet = c.NumeroCarnet,
        Nombre = c.Nombre,
        Apellidos = c.Apellidos,
        Username = c.Username,
        Activo = c.Activo
      });

      var response = new GetClientesByIdNegocioResponse
      {
        Clientes = clienteDtos
      };

      await SendOkAsync(response, ct);
    }
  }
}
