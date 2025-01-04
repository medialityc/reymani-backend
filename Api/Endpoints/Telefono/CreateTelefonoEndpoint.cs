using System;
using reymani_web_api.Domain.Enums;

namespace reymani_web_api.Api.Endpoints.Telefono;

public sealed class CreateTelefonoEndpoint : Endpoint<CreateTelefonoRequest>
{
  private readonly ITelefonoService _telefonoService;
  private readonly IAuthorizationService _authorizationService;
  private readonly IClienteService _clienteService;
  private readonly INegocioService _negocioService;

  public CreateTelefonoEndpoint(ITelefonoService telefonoService, IAuthorizationService authorizationService, IClienteService clienteService, INegocioService negocioService)
  {
    _telefonoService = telefonoService;
    _authorizationService = authorizationService;
    _clienteService = clienteService;
    _negocioService = negocioService;
  }

  public override void Configure()
  {
    Verbs(Http.POST);
    Routes("/telefono");
    Summary(s =>
    {
      s.Summary = "Crear Teléfono";
      s.Description = "Crea un nuevo teléfono";
      s.ExampleRequest = new CreateTelefonoRequest
      {
        Numero = "1234567890",
        TipoEntidad = EntitiesTypes.Negocio.ToString(),
        IdEntidad = Guid.NewGuid(),
        Descripcion = "Teléfono de prueba"
      };
    });
  }

  public override async Task HandleAsync(CreateTelefonoRequest req, CancellationToken ct)
  {
    var roleGuids = User.Claims.Where(c => c.Type == "role").Select(c => Guid.Parse(c.Value)).ToArray();

    if (!await _authorizationService.IsRoleAuthorizedToEndpointAsync(roleGuids, "Crear_Telefono"))
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

    var existingTelefono = await _telefonoService.GetByNumeroAndEntidadAsync(req.Numero, req.IdEntidad);
    if (existingTelefono != null)
    {
      AddError(r => r.Numero, "El número de teléfono ya existe para esta entidad.");
    }

    ThrowIfAnyErrors();

    var telefono = new Domain.Entities.Telefono
    {
      IdTelefono = Guid.NewGuid(),
      NumeroTelefono = req.Numero,
      TipoEntidad = req.TipoEntidad.ToString(),
      IdEntidad = req.IdEntidad,
      Descripcion = req.Descripcion
    };

    await _telefonoService.AddAsync(telefono);

    await SendOkAsync(telefono, ct);
  }
}
