using System;
using reymani_web_api.Application.DTOs;

namespace reymani_web_api.Api.Endpoints.Usuario
{
  public class GetNegociosByIdUsuarioEndpoint : Endpoint<GetNegociosByIdUsuarioRequest, GetNegociosByIdUsuarioResponse>
  {
    private readonly INegocioUsuarioService _negocioUsuarioService;
    private readonly IAuthorizationService _authorizationService;
    private readonly IUsuarioService _UsuarioService;

    public GetNegociosByIdUsuarioEndpoint(INegocioUsuarioService negocioUsuarioService, IAuthorizationService authorizationService, IUsuarioService UsuarioService)
    {
      _negocioUsuarioService = negocioUsuarioService;
      _authorizationService = authorizationService;
      _UsuarioService = UsuarioService;
    }

    public override void Configure()
    {
      Verbs(Http.GET);
      Routes("/usuario/{IdUsuario:guid}/negocios");
      Summary(s =>
      {
        s.Summary = "Obtener todos los negocios por ID de Usuario";
        s.Description = "Obtiene todos los negocios de la base de datos por el ID del Usuario";
        s.ExampleRequest = new GetNegociosByIdUsuarioRequest
        {
          IdUsuario = Guid.NewGuid()
        };
        s.ResponseExamples[200] = new GetNegociosByIdUsuarioResponse
        {
          Negocios = new List<NegocioDto>
          {
            new NegocioDto
            {
              IdNegocio = Guid.NewGuid(),
              Nombre = "Negocio Ejemplo",
              Descripcion = "Descripción Ejemplo",
              EntregaDomicilio = true
            }
          }
        };
        s.Responses[404] = "Negocios no encontrados";
        s.Responses[200] = "Negocios encontrados";
      });
    }

    public override async Task HandleAsync(GetNegociosByIdUsuarioRequest req, CancellationToken ct)
    {
      var roleGuids = User.Claims.Where(c => c.Type == "role").Select(c => Guid.Parse(c.Value)).ToArray();

      if (!await _authorizationService.IsRoleAuthorizedToEndpointAsync(roleGuids, "Ver_Negocios_Del_Usuario"))
      {
        await SendUnauthorizedAsync(ct);
      }

      var Usuario = await _UsuarioService.GetUsuarioByIdAsync(req.IdUsuario);
      if (Usuario == null)
      {
        AddError(req => req.IdUsuario, "Usuario no encontrado");
      }

      ThrowIfAnyErrors();

      var negocios = await _negocioUsuarioService.GetNegociosByUsuarioIdAsync(req.IdUsuario);
      var negocioDtos = negocios.Select(n => new NegocioDto
      {
        IdNegocio = n.IdNegocio,
        Nombre = n.Nombre,
        Descripcion = n.Descripcion,
        EntregaDomicilio = n.EntregaDomicilio
      });

      var response = new GetNegociosByIdUsuarioResponse
      {
        Negocios = negocioDtos
      };

      await SendOkAsync(response, ct);
    }
  }
}
