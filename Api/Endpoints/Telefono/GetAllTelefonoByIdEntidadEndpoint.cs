using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using reymani_web_api.Application.Interfaces;
using reymani_web_api.Domain.Entities;

namespace reymani_web_api.Api.Endpoints.Telefono
{
  public class GetAllTelefonoByIdEntidadEndpoint : Endpoint<GetAllTelefonoByIdEntidadRequest, IEnumerable<Domain.Entities.Telefono>>
  {
    private readonly ITelefonoService _telefonoService;
    private readonly IAuthorizationService _authorizationService;

    public GetAllTelefonoByIdEntidadEndpoint(ITelefonoService telefonoService, IAuthorizationService authorizationService)
    {
      _telefonoService = telefonoService;
      _authorizationService = authorizationService;
    }

    public override void Configure()
    {
      Verbs(Http.GET);
      Routes("/telefono/entidad/{IdEntidad:guid}");
      Summary(s =>
      {
        s.Summary = "Obtener todos los teléfonos por ID de entidad";
        s.Description = "Obtiene todos los teléfonos de la base de datos por el ID de la entidad";
        s.ExampleRequest = new GetAllTelefonoByIdEntidadRequest
        {
          IdEntidad = Guid.NewGuid()
        };
        s.ResponseExamples[200] = new List<Domain.Entities.Telefono>
        {
          new Domain.Entities.Telefono
          {
            IdTelefono = Guid.NewGuid(),
            NumeroTelefono = "123456789",
            IdEntidad = Guid.NewGuid(),
            TipoEntidad = "Usuario",
            Descripcion = "Teléfono de ejemplo"
          }
        };
        s.Responses[404] = "Teléfonos no encontrados";
        s.Responses[200] = "Teléfonos encontrados";
      });
    }

    public override async Task HandleAsync(GetAllTelefonoByIdEntidadRequest req, CancellationToken ct)
    {
      var roleGuids = User.Claims.Where(c => c.Type == "role").Select(c => Guid.Parse(c.Value)).ToArray();

      if (!await _authorizationService.IsRoleAuthorizedToEndpointAsync(roleGuids, "Ver_Telefonos_Entidad"))
      {
        await SendUnauthorizedAsync(ct);
      }

      var telefonos = await _telefonoService.GetAllByIdEntidadAsync(req.IdEntidad);
      if (telefonos == null || !telefonos.Any())
      {
        await SendNotFoundAsync(ct);
        return;
      }

      await SendOkAsync(telefonos, ct);
    }
  }
}
