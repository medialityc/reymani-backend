using System;
using reymani_web_api.Application.DTOs;

namespace reymani_web_api.Api.Endpoints.Usuario;

public class UpdateUsuarioEndpoint : Endpoint<UpdateUsuarioRequest>
{
  private readonly IUsuarioService _UsuarioService;

  private readonly IAuthorizationService _authorizationService;

  public UpdateUsuarioEndpoint(IUsuarioService UsuarioService, IAuthorizationService authorizationService)
  {
    _UsuarioService = UsuarioService;
    _authorizationService = authorizationService;
  }

  public override void Configure()
  {
    Verbs(Http.PUT);
    Routes("/usuario/{IdUsuario:guid}");
    Summary(s =>
    {
      s.Summary = "Actualizar Usuario";
      s.Description = "Actualiza un Usuario en la base de datos";
      s.ExampleRequest = new UpdateUsuarioRequest
      {
        IdUsuario = Guid.NewGuid(),
        Usuario = new UsuarioDto
        {
          NumeroCarnet = "04022067256",
          Nombre = "John",
          Apellidos = "Doe",
          Username = "johndoe",
          Activo = true
        }
      };
    });

  }

  public override async Task HandleAsync(UpdateUsuarioRequest req, CancellationToken ct)
  {
    var roleGuids = User.Claims.Where(c => c.Type == "role").Select(c => Guid.Parse(c.Value)).ToArray();

    if (!await _authorizationService.IsRoleAuthorizedToEndpointAsync(roleGuids, "Actualizar_Usuario"))
    {
      await SendUnauthorizedAsync(ct);
    }

    var Usuario = await _UsuarioService.GetUsuarioByIdAsync(req.IdUsuario);
    if (Usuario == null)
    {
      await SendNotFoundAsync(ct);
      return;
    }

    if (req.Usuario.Id != req.IdUsuario)
    {
      AddError(r => r.IdUsuario, "El ID del Usuario no coincide con el ID de la URL");
    }

    ThrowIfAnyErrors();

    if (Usuario != null)
    {
      Usuario.NumeroCarnet = req.Usuario.NumeroCarnet;
      Usuario.Nombre = req.Usuario.Nombre;
      Usuario.Apellidos = req.Usuario.Apellidos;
      Usuario.Username = req.Usuario.Username;
      await _UsuarioService.UpdateUsuarioAsync(Usuario);
      await SendOkAsync(ct);
    }
  }
}