using System;
using reymani_web_api.Application.DTOs;

namespace reymani_web_api.Api.Endpoints.Negocio;

public class GetUsuariosByIdNegocioResponse
{
  public required IEnumerable<UsuarioDto> Usuarios { get; set; }
}
