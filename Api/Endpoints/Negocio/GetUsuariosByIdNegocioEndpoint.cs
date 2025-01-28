
using reymani_web_api.Application.DTOs;

namespace reymani_web_api.Api.Endpoints.Negocio
{
  public class GetUsuariosByIdNegocioEndpoint : Endpoint<GetUsuariosByIdNegocioRequest, GetUsuariosByIdNegocioResponse>
  {
    private readonly INegocioUsuarioService _negocioUsuarioService;
    private readonly IAuthorizationService _authorizationService;
    private readonly INegocioService _negocioService;

    public GetUsuariosByIdNegocioEndpoint(INegocioUsuarioService negocioUsuarioService, IAuthorizationService authorizationService, INegocioService negocioService)
    {
      _negocioUsuarioService = negocioUsuarioService;
      _authorizationService = authorizationService;
      _negocioService = negocioService;
    }

    public override void Configure()
    {
      Verbs(Http.GET);
      Routes("/negocio/{IdNegocio:guid}/usuarios");
      Summary(s =>
      {
        s.Summary = "Obtener todos los Usuarios por ID de negocio";
        s.Description = "Obtiene todos los Usuarios de la base de datos por el ID del negocio";
        s.ExampleRequest = new GetUsuariosByIdNegocioRequest
        {
          IdNegocio = Guid.NewGuid()
        };
        s.ResponseExamples[200] = new GetUsuariosByIdNegocioResponse
        {
          Usuarios = new List<UsuarioDto>
          {
            new UsuarioDto
            {
              Id = Guid.NewGuid(),
              NumeroCarnet = "12345678901",
              Nombre = "Usuario Ejemplo",
              Apellidos = "Apellido Ejemplo",
              Username = "Usuarioejemplo",
              Activo = true
            }
          }
        };
        s.Responses[404] = "Usuarios no encontrados";
        s.Responses[200] = "Usuarios encontrados";
      });
    }

    public override async Task HandleAsync(GetUsuariosByIdNegocioRequest req, CancellationToken ct)
    {
      var roleGuids = User.Claims.Where(c => c.Type == "role").Select(c => Guid.Parse(c.Value)).ToArray();

      if (!await _authorizationService.IsRoleAuthorizedToEndpointAsync(roleGuids, "Ver_Usuarios_Del_Negocio"))
      {
        await SendUnauthorizedAsync(ct);
      }

      var negocio = await _negocioService.GetByIdAsync(req.IdNegocio);
      if (negocio == null)
      {
        AddError(req => req.IdNegocio, "Negocio no encontrado");
      }

      ThrowIfAnyErrors();

      var Usuarios = await _negocioUsuarioService.GetUsuariosByNegocioIdAsync(req.IdNegocio);
      var UsuarioDtos = Usuarios.Select(c => new UsuarioDto
      {
        Id = c.IdUsuario,
        NumeroCarnet = c.NumeroCarnet,
        Nombre = c.Nombre,
        Apellidos = c.Apellidos,
        Username = c.Username,
        Activo = c.Activo
      });

      var response = new GetUsuariosByIdNegocioResponse
      {
        Usuarios = UsuarioDtos
      };

      await SendOkAsync(response, ct);
    }
  }
}
