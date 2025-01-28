using System;
using reymani_web_api.Application.DTOs;

namespace reymani_web_api.Api.Endpoints.Telefono
{
  public class GetTelefonoByIdEndpoint : Endpoint<GetTelefonoByIdRequest, Domain.Entities.Telefono>
  {
    private readonly ITelefonoService _telefonoService;
    private readonly IAuthorizationService _authorizationService;

    public GetTelefonoByIdEndpoint(ITelefonoService telefonoService, IAuthorizationService authorizationService)
    {
      _telefonoService = telefonoService;
      _authorizationService = authorizationService;
    }

    public override void Configure()
    {
      Verbs(Http.GET);
      Routes("/telefono/{TelefonoId:guid}");
      Summary(s =>
      {
        s.Summary = "Obtener teléfono por ID";
        s.Description = "Obtiene un teléfono de la base de datos por su ID";
        s.ExampleRequest = new GetTelefonoByIdRequest
        {
          TelefonoId = Guid.NewGuid()
        };
        s.ResponseExamples[200] = new Domain.Entities.Telefono
        {
          IdTelefono = Guid.NewGuid(),
          NumeroTelefono = "123456789",
          IdEntidad = Guid.NewGuid(),
          TipoEntidad = "Usuario",
          Descripcion = "Teléfono de ejemplo"
        };
        s.Responses[404] = "Teléfono no encontrado";
        s.Responses[200] = "Teléfono encontrado";
      });
    }

    public override async Task HandleAsync(GetTelefonoByIdRequest req, CancellationToken ct)
    {
      var roleGuids = User.Claims.Where(c => c.Type == "role").Select(c => Guid.Parse(c.Value)).ToArray();

      if (!await _authorizationService.IsRoleAuthorizedToEndpointAsync(roleGuids, "Ver_Telefono"))
      {
        await SendUnauthorizedAsync(ct);
      }

      var telefono = await _telefonoService.GetByIdAsync(req.TelefonoId);
      if (telefono == null)
      {
        await SendNotFoundAsync(ct);
        return;
      }

      var telefonoDto = new Domain.Entities.Telefono
      {
        IdTelefono = telefono.IdTelefono,
        NumeroTelefono = telefono.NumeroTelefono,
        IdEntidad = telefono.IdEntidad,
        TipoEntidad = telefono.TipoEntidad.ToString(),
        Descripcion = telefono.Descripcion
      };

      await SendOkAsync(telefonoDto, ct);
    }
  }
}
