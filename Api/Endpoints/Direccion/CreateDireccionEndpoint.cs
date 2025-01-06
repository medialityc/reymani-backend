using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using reymani_web_api.Application.Interfaces;
using reymani_web_api.Domain.Enums;

namespace reymani_web_api.Api.Endpoints.Direccion;

public sealed class CreateDireccionEndpoint : Endpoint<CreateDireccionRequest>
{
  private readonly IDireccionService _direccionService;
  private readonly IAuthorizationService _authorizationService;
  private readonly IClienteService _clienteService;
  private readonly INegocioService _negocioService;

  public CreateDireccionEndpoint(IDireccionService direccionService, IAuthorizationService authorizationService, IClienteService clienteService, INegocioService negocioService)
  {
    _direccionService = direccionService;
    _authorizationService = authorizationService;
    _clienteService = clienteService;
    _negocioService = negocioService;
  }

  public override void Configure()
  {
    Verbs(Http.POST);
    Routes("/direccion");
    Summary(s =>
    {
      s.Summary = "Crear Dirección";
      s.Description = "Crea una nueva dirección";
      s.ExampleRequest = new CreateDireccionRequest
      {
        TipoEntidad = EntitiesTypes.Negocio.ToString(),
        IdEntidad = Guid.NewGuid(),
        DireccionEntidad = "Calle Falsa 123",
        Municipio = "Springfield",
        Provincia = "Illinois",
        Latitud = 39.7817,
        Longitud = -89.6501,
        Descripcion = "Dirección de prueba"
      };
    });
  }

  public override async Task HandleAsync(CreateDireccionRequest req, CancellationToken ct)
  {
    var roleGuids = User.Claims.Where(c => c.Type == "role").Select(c => Guid.Parse(c.Value)).ToArray();

    if (!await _authorizationService.IsRoleAuthorizedToEndpointAsync(roleGuids, "Crear_Direccion"))
    {
      await SendUnauthorizedAsync(ct);
    }

    if (!Enum.TryParse(typeof(EntitiesTypes), req.TipoEntidad, true, out var tipoEntidad))
    {
      AddError(r => r.TipoEntidad, "Tipo de entidad inválido");
    }

    if (tipoEntidad != null && (EntitiesTypes)tipoEntidad == EntitiesTypes.Cliente)
    {
      var cliente = await _clienteService.GetClienteByIdAsync(req.IdEntidad);
      if (cliente == null)
      {
        AddError(r => r.IdEntidad, "Cliente no encontrado");
      }
    }
    else if (tipoEntidad != null && (EntitiesTypes)tipoEntidad == EntitiesTypes.Negocio)
    {
      var negocio = await _negocioService.GetByIdAsync(req.IdEntidad);
      if (negocio == null)
      {
        AddError(r => r.IdEntidad, "Negocio no encontrado");
      }
    }

    ThrowIfAnyErrors();

    var direccion = new Domain.Entities.Direccion
    {
      IdDireccion = Guid.NewGuid(),
      TipoEntidad = req.TipoEntidad,
      IdEntidad = req.IdEntidad,
      DireccionEntidad = req.DireccionEntidad,
      Municipio = req.Municipio,
      Provincia = req.Provincia,
      Latitud = req.Latitud,
      Longitud = req.Longitud,
      Descripcion = req.Descripcion
    };

    await _direccionService.AddAsync(direccion);

    await SendOkAsync(direccion, ct);
  }
}
