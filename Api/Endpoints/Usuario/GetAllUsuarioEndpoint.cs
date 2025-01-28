using System;
using reymani_web_api.Application.DTOs;

namespace reymani_web_api.Api.Endpoints.Usuario;

public class GetAllUsuarioEndpoint : EndpointWithoutRequest<GetAllUsuarioResponse>
{
  private readonly IUsuarioService _UsuarioService;
  private readonly IAuthorizationService _authorizationService;

  public GetAllUsuarioEndpoint(IUsuarioService UsuarioService, IAuthorizationService authorizationService)
  {
    _UsuarioService = UsuarioService;
    _authorizationService = authorizationService;
  }

  public override void Configure()
  {
    Get("/usuario");
    Summary(s =>
    {
      s.Summary = "Obtener todos los Usuarios";
      s.Description = "Obtiene todos los Usuarios de la base de datos";
      s.ResponseExamples[200] = new GetAllUsuarioResponse
      {
        Usuarios = new List<UsuarioDto>
        {
          new UsuarioDto
          {
            Id = Guid.NewGuid(),
            NumeroCarnet = "12345678",
            Nombre = "John",
            Apellidos = "Doe",
            Username = "johndoe",
            Activo = true
          }
        }
      };
    });
  }

  public override async Task<GetAllUsuarioResponse> ExecuteAsync(CancellationToken ct)
  {
    var roleGuids = User.Claims.Where(c => c.Type == "role").Select(c => Guid.Parse(c.Value)).ToArray();

    if (!await _authorizationService.IsRoleAuthorizedToEndpointAsync(roleGuids, "Ver_Usuarios"))
    {
      await SendUnauthorizedAsync(ct);
    }

    var Usuarios = await _UsuarioService.GetAllUsuariosAsync();
    return new GetAllUsuarioResponse
    {
      Usuarios = Usuarios.Select(c => new UsuarioDto
      {
        Id = c.IdUsuario,
        NumeroCarnet = c.NumeroCarnet,
        Nombre = c.Nombre,
        Apellidos = c.Apellidos,
        Username = c.Username,
        Activo = c.Activo
      }).ToList()
    };
  }
}