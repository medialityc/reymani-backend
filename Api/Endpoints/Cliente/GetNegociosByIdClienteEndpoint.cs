using System;
using reymani_web_api.Application.DTOs;

namespace reymani_web_api.Api.Endpoints.Cliente
{
  public class GetNegociosByIdClienteEndpoint : Endpoint<GetNegociosByIdClienteRequest, GetNegociosByIdClienteResponse>
  {
    private readonly INegocioClienteService _negocioClienteService;
    private readonly IAuthorizationService _authorizationService;
    private readonly IClienteService _clienteService;

    public GetNegociosByIdClienteEndpoint(INegocioClienteService negocioClienteService, IAuthorizationService authorizationService, IClienteService clienteService)
    {
      _negocioClienteService = negocioClienteService;
      _authorizationService = authorizationService;
      _clienteService = clienteService;
    }

    public override void Configure()
    {
      Verbs(Http.GET);
      Routes("/cliente/{IdCliente:guid}/negocios");
      Summary(s =>
      {
        s.Summary = "Obtener todos los negocios por ID de cliente";
        s.Description = "Obtiene todos los negocios de la base de datos por el ID del cliente";
        s.ExampleRequest = new GetNegociosByIdClienteRequest
        {
          IdCliente = Guid.NewGuid()
        };
        s.ResponseExamples[200] = new GetNegociosByIdClienteResponse
        {
          Negocios = new List<NegocioDto>
          {
            new NegocioDto
            {
              IdNegocio = Guid.NewGuid(),
              Nombre = "Negocio Ejemplo",
              Descripcion = "Descripción Ejemplo",
              EntregaDomicilio = true,
              URLImagenPrincipal = "http://example.com/imagen.jpg",
              URLImagenLogo = "http://example.com/logo.jpg",
              URLImagenBanner = "http://example.com/banner.jpg"
            }
          }
        };
        s.Responses[404] = "Negocios no encontrados";
        s.Responses[200] = "Negocios encontrados";
      });
    }

    public override async Task HandleAsync(GetNegociosByIdClienteRequest req, CancellationToken ct)
    {
      var roleGuids = User.Claims.Where(c => c.Type == "role").Select(c => Guid.Parse(c.Value)).ToArray();

      if (!await _authorizationService.IsRoleAuthorizedToEndpointAsync(roleGuids, "Ver_Negocios_Del_Cliente"))
      {
        await SendUnauthorizedAsync(ct);
      }

      var cliente = await _clienteService.GetClienteByIdAsync(req.IdCliente);
      if (cliente == null)
      {
        AddError(req => req.IdCliente, "Cliente no encontrado");
      }

      ThrowIfAnyErrors();

      var negocios = await _negocioClienteService.GetNegociosByClienteIdAsync(req.IdCliente);
      var negocioDtos = negocios.Select(n => new NegocioDto
      {
        IdNegocio = n.IdNegocio,
        Nombre = n.Nombre,
        Descripcion = n.Descripcion,
        EntregaDomicilio = n.EntregaDomicilio,
        URLImagenPrincipal = n.URLImagenPrincipal,
        URLImagenLogo = n.URLImagenLogo,
        URLImagenBanner = n.URLImagenBanner
      });

      var response = new GetNegociosByIdClienteResponse
      {
        Negocios = negocioDtos
      };

      await SendOkAsync(response, ct);
    }
  }
}
