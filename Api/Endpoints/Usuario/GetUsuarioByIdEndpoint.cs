using System;
using reymani_web_api.Application.DTOs;
using reymani_web_api.Application.Services;

namespace reymani_web_api.Api.Endpoints.Usuario
{
  public class GetUsuarioByIdEndpoint : Endpoint<GetUsuarioByIdRequest, UsuarioDto>
  {
    private readonly IUsuarioService _UsuarioService;
    private readonly IAuthorizationService _authorizationService;

    public GetUsuarioByIdEndpoint(IUsuarioService UsuarioService, IAuthorizationService authorizationService)
    {
      _UsuarioService = UsuarioService;
      _authorizationService = authorizationService;
    }

    public override void Configure()
    {
      Verbs(Http.GET);
      Routes("/usuario/{UsuarioId:guid}");
      Summary(s =>
      {
        s.Summary = "Obtener Usuario por ID";
        s.Description = "Obtiene un Usuario de la base de datos por su ID";
        s.ExampleRequest = new GetUsuarioByIdRequest
        {
          UsuarioId = Guid.NewGuid()
        };
        s.ResponseExamples[200] = new UsuarioDto
        {
          Id = Guid.NewGuid(),
          NumeroCarnet = "12345678",
          Nombre = "John",
          Apellidos = "Doe",
          Username = "johndoe",
          Activo = true
        };
        s.Responses[404] = "Usuario no encontrado";
        s.Responses[200] = "Usuario encontrado";
      });
    }

    public override async Task HandleAsync(GetUsuarioByIdRequest req, CancellationToken ct)
    {
      var roleGuids = User.Claims.Where(c => c.Type == "role").Select(c => Guid.Parse(c.Value)).ToArray();

      if (!await _authorizationService.IsRoleAuthorizedToEndpointAsync(roleGuids, "Ver_Usuario"))
      {
        await SendUnauthorizedAsync(ct);
      }

      var Usuario = await _UsuarioService.GetUsuarioByIdAsync(req.UsuarioId);
      if (Usuario == null)
      {
        await SendNotFoundAsync(ct);
        return;
      }

      var UsuarioDto = new UsuarioDto
      {
        Id = Usuario.IdUsuario,
        NumeroCarnet = Usuario.NumeroCarnet,
        Nombre = Usuario.Nombre,
        Apellidos = Usuario.Apellidos,
        Username = Usuario.Username,
        Activo = Usuario.Activo
      };

      await SendOkAsync(UsuarioDto, ct);
    }
  }
}
