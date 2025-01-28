using System;
using reymani_web_api.Application.DTOs;

namespace reymani_web_api.Api.Endpoints.Usuario;

public class GetAllUsuarioResponse
{
  public List<UsuarioDto> Usuarios { get; set; } = new List<UsuarioDto>();
}