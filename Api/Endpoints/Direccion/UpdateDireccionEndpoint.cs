using System;
using reymani_web_api.Domain.Enums;

namespace reymani_web_api.Api.Endpoints.Direccion;

public class UpdateDireccionEndpoint : Endpoint<UpdateDireccionRequest>
{
  private readonly IDireccionService _direccionService;
  private readonly IAuthorizationService _authorizationService;
  private readonly IUsuarioService _UsuarioService;
  private readonly INegocioService _negocioService;

  public UpdateDireccionEndpoint(IDireccionService direccionService, IAuthorizationService authorizationService, IUsuarioService UsuarioService, INegocioService negocioService)
  {
    _direccionService = direccionService;
    _authorizationService = authorizationService;
    _UsuarioService = UsuarioService;
    _negocioService = negocioService;
  }

  public override void Configure()
  {
    Verbs(Http.PUT);
    Routes("/direccion/{IdDireccion:guid}");
    Summary(s =>
    {
      s.Summary = "Actualizar Dirección";
      s.Description = "Actualiza una dirección en la base de datos";
      s.ExampleRequest = new UpdateDireccionRequest
      {
        IdDireccion = Guid.NewGuid(),
        Direccion = new Domain.Entities.Direccion
        {
          DireccionEntidad = "Calle Falsa 123",
          IdEntidad = Guid.NewGuid(),
          Descripcion = "Dirección de ejemplo",
          TipoEntidad = "Negocio",
          Municipio = "Springfield",
          Provincia = "Springfield",
          Latitud = 0.0,
          Longitud = 0.0
        }
      };
    });
  }

  public override async Task HandleAsync(UpdateDireccionRequest req, CancellationToken ct)
  {
    var roleGuids = User.Claims.Where(c => c.Type == "role").Select(c => Guid.Parse(c.Value)).ToArray();

    if (!await _authorizationService.IsRoleAuthorizedToEndpointAsync(roleGuids, "Actualizar_Direccion"))
    {
      await SendUnauthorizedAsync(ct);
    }

    var direccion = await _direccionService.GetByIdAsync(req.IdDireccion);
    if (direccion == null)
    {
      await SendNotFoundAsync(ct);
      return;
    }

    if (req.Direccion.IdDireccion != req.IdDireccion)
    {
      AddError(r => r.IdDireccion, "El ID de la dirección no coincide con el ID de la URL");
    }

    if (!Enum.TryParse(typeof(EntitiesTypes), req.Direccion.TipoEntidad, true, out var tipoEntidad))
    {
      AddError(r => r.Direccion.TipoEntidad, "Tipo de entidad inválido");
    }

    ThrowIfAnyErrors();

    if (tipoEntidad != null && (EntitiesTypes)tipoEntidad == EntitiesTypes.Usuario)
    {
      var Usuario = await _UsuarioService.GetUsuarioByIdAsync(req.Direccion.IdEntidad);
      if (Usuario == null)
      {
        AddError(r => r.Direccion.IdEntidad, "Usuario no encontrado");
      }
    }
    else if (tipoEntidad != null && (EntitiesTypes)tipoEntidad == EntitiesTypes.Negocio)
    {
      var negocio = await _negocioService.GetByIdAsync(req.Direccion.IdEntidad);
      if (negocio == null)
      {
        AddError(r => r.Direccion.IdEntidad, "Negocio no encontrado");
      }
    }

    ThrowIfAnyErrors();

    if (direccion != null)
    {
      direccion.DireccionEntidad = req.Direccion.DireccionEntidad;
      direccion.IdEntidad = req.Direccion.IdEntidad;
      direccion.Descripcion = req.Direccion.Descripcion;
      direccion.Municipio = req.Direccion.Municipio;
      direccion.Provincia = req.Direccion.Provincia;
      direccion.Latitud = req.Direccion.Latitud;
      direccion.Longitud = req.Direccion.Longitud;
      await _direccionService.UpdateAsync(direccion);
      await SendOkAsync(ct);
    }
  }
}
