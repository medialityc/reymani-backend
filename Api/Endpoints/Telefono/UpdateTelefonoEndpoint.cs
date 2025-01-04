using System;
using reymani_web_api.Domain.Enums;

namespace reymani_web_api.Api.Endpoints.Telefono;

public class UpdateTelefonoEndpoint : Endpoint<UpdateTelefonoRequest>
{
  private readonly ITelefonoService _telefonoService;
  private readonly IAuthorizationService _authorizationService;
  private readonly IClienteService _clienteService;
  private readonly INegocioService _negocioService;

  public UpdateTelefonoEndpoint(ITelefonoService telefonoService, IAuthorizationService authorizationService, IClienteService clienteService, INegocioService negocioService)
  {
    _telefonoService = telefonoService;
    _authorizationService = authorizationService;
    _clienteService = clienteService;
    _negocioService = negocioService;
  }

  public override void Configure()
  {
    Verbs(Http.PUT);
    Routes("/telefono/{IdTelefono:guid}");
    Summary(s =>
    {
      s.Summary = "Actualizar Teléfono";
      s.Description = "Actualiza un teléfono en la base de datos";
      s.ExampleRequest = new UpdateTelefonoRequest
      {
        IdTelefono = Guid.NewGuid(),
        Telefono = new Domain.Entities.Telefono
        {
          NumeroTelefono = "123456789",
          IdEntidad = Guid.NewGuid(),
          Descripcion = "Teléfono de ejemplo",
          TipoEntidad = "Negocio"
        }
      };
    });
  }

  public override async Task HandleAsync(UpdateTelefonoRequest req, CancellationToken ct)
  {
    var roleGuids = User.Claims.Where(c => c.Type == "role").Select(c => Guid.Parse(c.Value)).ToArray();

    if (!await _authorizationService.IsRoleAuthorizedToEndpointAsync(roleGuids, "Actualizar_Telefono"))
    {
      await SendUnauthorizedAsync(ct);
    }

    var telefono = await _telefonoService.GetByIdAsync(req.IdTelefono);
    if (telefono == null)
    {
      await SendNotFoundAsync(ct);
      return;
    }

    if (req.Telefono.IdTelefono != req.IdTelefono)
    {
      AddError(r => r.IdTelefono, "El ID del teléfono no coincide con el ID de la URL");
    }

    if (!Enum.TryParse(typeof(EntitiesTypes), req.Telefono.TipoEntidad, true, out var tipoEntidad))
    {
      AddError(r => r.Telefono.TipoEntidad, "Tipo de entidad inválido");
    }

    ThrowIfAnyErrors();

    if (tipoEntidad != null && (EntitiesTypes)tipoEntidad == EntitiesTypes.Cliente)
    {
      var cliente = await _clienteService.GetClienteByIdAsync(req.Telefono.IdEntidad);
      if (cliente == null)
      {
        AddError(r => r.Telefono.IdEntidad, "Cliente no encontrado");
      }
    }
    else if (tipoEntidad != null && (EntitiesTypes)tipoEntidad == EntitiesTypes.Negocio)
    {
      var negocio = await _negocioService.GetByIdAsync(req.Telefono.IdEntidad);
      if (negocio == null)
      {
        AddError(r => r.Telefono.IdEntidad, "Negocio no encontrado");
      }
    }

    ThrowIfAnyErrors();

    var existingTelefono = await _telefonoService.GetByNumeroAndEntidadAsync(req.Telefono.NumeroTelefono, req.Telefono.IdEntidad);
    if (existingTelefono != null && existingTelefono.IdTelefono != req.Telefono.IdTelefono)
    {
      AddError(r => r.Telefono.NumeroTelefono, "El número de teléfono ya existe para esta entidad.");
    }

    ThrowIfAnyErrors();

    if (telefono != null)
    {
      telefono.NumeroTelefono = req.Telefono.NumeroTelefono;
      telefono.IdEntidad = req.Telefono.IdEntidad;
      telefono.Descripcion = req.Telefono.Descripcion;
      await _telefonoService.UpdateAsync(telefono);
      await SendOkAsync(ct);
    }
  }
}
