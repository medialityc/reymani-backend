using System;
using reymani_web_api.Domain.Enums;

namespace reymani_web_api.Api.Endpoints.Telefono;

public sealed class CreateTelefonoEndpoint : Endpoint<CreateTelefonoRequest>
{
  private readonly ITelefonoService _telefonoService;
  private readonly IAuthorizationService _authorizationService;

  public CreateTelefonoEndpoint(ITelefonoService telefonoService, IAuthorizationService authorizationService)
  {
    _telefonoService = telefonoService;
    _authorizationService = authorizationService;
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

    if (!Enum.TryParse(typeof(EntitiesTypes), req.TipoEntidad, out var tipoEntidad))
    {
      AddError(r => r.IdEntidad, "Tipo de entidad inválido");
    }

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
