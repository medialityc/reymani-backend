using reymani_web_api.Domain.Enums;

namespace reymani_web_api.Api.Endpoints.MetodoPago;

public class UpdateMetodoPagoEndpoint : Endpoint<UpdateMetodoPagoRequest>
{
  private readonly IMetodoPagoService _metodoPagoService;
  private readonly IAuthorizationService _authorizationService;
  private readonly IClienteService _clienteService;
  private readonly INegocioService _negocioService;

  public UpdateMetodoPagoEndpoint(IMetodoPagoService metodoPagoService, IAuthorizationService authorizationService, INegocioService negocioService, IClienteService clienteService)
  {
    _metodoPagoService = metodoPagoService;
    _authorizationService = authorizationService;
    _negocioService = negocioService;
    _clienteService = clienteService;
  }

  public override void Configure()
  {
    Verbs(Http.PUT);
    Routes("/metodo-pago/{IdMetodoPago:guid}");
    Summary(s =>
    {
      s.Summary = "Actualizar Método de Pago";
      s.Description = "Actualiza un método de pago en la base de datos";
      s.ExampleRequest = new UpdateMetodoPagoRequest
      {
        IdMetodoPago = Guid.NewGuid(),
        TipoEntidad = "Negocio",
        IdEntidad = Guid.NewGuid(),
        Proveedor = "Proveedor Actualizado",
        FechaExpiracion = DateTime.UtcNow.AddYears(1),
        Activo = true,
      };
    });
    Validator<UpdateMetodoPagoRequestValidator>();
  }

  public override async Task HandleAsync(UpdateMetodoPagoRequest req, CancellationToken ct)
  {
    var roleGuids = User.Claims.Where(c => c.Type == "role").Select(c => Guid.Parse(c.Value)).ToArray();

    if (!await _authorizationService.IsRoleAuthorizedToEndpointAsync(roleGuids, "Actualizar_Metodo_Pago"))
    {
      await SendUnauthorizedAsync(ct);
    }

    var metodoPago = await _metodoPagoService.GetByIdAsync(req.IdMetodoPago);
    if (metodoPago == null)
    {
      await SendNotFoundAsync(ct);
      return;
    }

    if (await _metodoPagoService.ExistsByEntidadAndProveedorAsync(req.IdEntidad, req.Proveedor))
    {
      AddError(r => r.Proveedor, "Ya existe un método de pago con este proveedor para la entidad dada.");
      ThrowIfAnyErrors();
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

    metodoPago.TipoEntidad = req.TipoEntidad;
    metodoPago.IdEntidad = req.IdEntidad;
    metodoPago.Proveedor = req.Proveedor;
    metodoPago.FechaExpiracion = req.FechaExpiracion;
    metodoPago.Activo = req.Activo;
    metodoPago.Dato1 = req.Dato1;
    metodoPago.Dato2 = req.Dato2;
    metodoPago.Dato3 = req.Dato3;

    await _metodoPagoService.UpdateAsync(metodoPago);
    await SendOkAsync(ct);
  }
}