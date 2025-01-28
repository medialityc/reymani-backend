using System;
using reymani_web_api.Application.DTOs;

namespace reymani_web_api.Api.Endpoints.Usuario
{
  public class GetNegociosByIdUsuarioResponse
  {
    public required IEnumerable<NegocioDto> Negocios { get; set; }
  }
}
