using reymani_web_api.Domain.Enums;

namespace reymani_web_api.Api.Endpoints.MetodoPago;

public sealed class CreateMetodoPagoEndpoint : Endpoint<CreateMetodoPagoRequest>
{
  private readonly IMetodoPagoService _metodoPagoService;
  private readonly IAuthorizationService _authorizationService;
  private readonly IClienteService _clienteService;
  private readonly INegocioService _negocioService;

  public CreateMetodoPagoEndpoint(IMetodoPagoService metodoPagoService, IAuthorizationService authorizationService, IClienteService clienteService, INegocioService negocioService)
  {
    _metodoPagoService = metodoPagoService;
    _authorizationService = authorizationService;
    _clienteService = clienteService;
    _negocioService = negocioService;
  }

  public override void Configure()
  {
    Verbs(Http.POST);
    Routes("/metodo-pago");
    Summary(s =>
    {
      s.Summary = "Crear Método de Pago";
      s.Description = "Crea un nuevo método de pago";
      s.ExampleRequest = new CreateMetodoPagoRequest
      {
        TipoEntidad = "Negocio",
        IdEntidad = Guid.NewGuid(),
        Proveedor = "Proveedor Ejemplo",
        FechaExpiracion = DateTime.UtcNow.AddYears(1)
      };
    });
    Validator<CreateMetodoPagoRequestValidator>();
  }

  public override async Task HandleAsync(CreateMetodoPagoRequest req, CancellationToken ct)
  {
    var roleGuids = User.Claims.Where(c => c.Type == "role").Select(c => Guid.Parse(c.Value)).ToArray();

    if (!await _authorizationService.IsRoleAuthorizedToEndpointAsync(roleGuids, "Crear_Metodo_Pago"))
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

    if (await _metodoPagoService.ExistsByEntidadAndProveedorAsync(req.IdEntidad, req.Proveedor))
    {
      AddError(r => r.Proveedor, "Ya existe un método de pago con este proveedor para la entidad dada.");
      ThrowIfAnyErrors();
    }

    ThrowIfAnyErrors();

    var metodoPago = new Domain.Entities.MetodoPago
    {
      IdMetodoPago = Guid.NewGuid(),
      TipoEntidad = req.TipoEntidad,
      IdEntidad = req.IdEntidad,
      Proveedor = req.Proveedor,
      FechaExpiracion = req.FechaExpiracion,
      Activo = true,
      FechaRegistro = DateTime.UtcNow,
      Dato1 = req.Dato1,
      Dato2 = req.Dato2,
      Dato3 = req.Dato3
    };

    await _metodoPagoService.AddAsync(metodoPago);

    await SendOkAsync(metodoPago, ct);
  }
}