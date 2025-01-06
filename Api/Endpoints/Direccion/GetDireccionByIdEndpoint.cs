using System;
using reymani_web_api.Application.DTOs;

namespace reymani_web_api.Api.Endpoints.Direccion;

public class GetDireccionByIdEndpoint : Endpoint<GetDireccionByIdRequest, Domain.Entities.Direccion>
{
  private readonly IDireccionService _direccionService;
  private readonly IAuthorizationService _authorizationService;

  public GetDireccionByIdEndpoint(IDireccionService direccionService, IAuthorizationService authorizationService)
  {
    _direccionService = direccionService;
    _authorizationService = authorizationService;
  }

  public override void Configure()
  {
    Verbs(Http.GET);
    Routes("/direccion/{DireccionId:guid}");
    Summary(s =>
    {
      s.Summary = "Obtener dirección por ID";
      s.Description = "Obtiene una dirección de la base de datos por su ID";
      s.ExampleRequest = new GetDireccionByIdRequest
      {
        DireccionId = Guid.NewGuid()
      };
      s.ResponseExamples[200] = new Domain.Entities.Direccion
      {
        IdDireccion = Guid.NewGuid(),
        TipoEntidad = "Cliente",
        IdEntidad = Guid.NewGuid(),
        DireccionEntidad = "Calle Falsa 123",
        Municipio = "Ciudad",
        Provincia = "Provincia",
        Latitud = 0.0,
        Longitud = 0.0,
        Descripcion = "Descripción de ejemplo"
      };
      s.Responses[404] = "Dirección no encontrada";
      s.Responses[200] = "Dirección encontrada";
    });
  }

  public override async Task HandleAsync(GetDireccionByIdRequest req, CancellationToken ct)
  {
    var roleGuids = User.Claims.Where(c => c.Type == "role").Select(c => Guid.Parse(c.Value)).ToArray();

    if (!await _authorizationService.IsRoleAuthorizedToEndpointAsync(roleGuids, "Ver_Direccion"))
    {
      await SendUnauthorizedAsync(ct);
    }

    var direccion = await _direccionService.GetByIdAsync(req.DireccionId);
    if (direccion == null)
    {
      await SendNotFoundAsync(ct);
      return;
    }

    var direccionDto = new Domain.Entities.Direccion
    {
      IdDireccion = direccion.IdDireccion,
      TipoEntidad = direccion.TipoEntidad,
      IdEntidad = direccion.IdEntidad,
      DireccionEntidad = direccion.DireccionEntidad,
      Municipio = direccion.Municipio,
      Provincia = direccion.Provincia,
      Latitud = direccion.Latitud,
      Longitud = direccion.Longitud,
      Descripcion = direccion.Descripcion
    };

    await SendOkAsync(direccionDto, ct);
  }
}
