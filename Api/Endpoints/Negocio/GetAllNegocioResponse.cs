using System;
using reymani_web_api.Application.DTOs;

namespace reymani_web_api.Api.Endpoints.Negocio;

public class GetAllNegocioResponse
{
  public List<NegocioDto> Negocios { get; set; } = new List<NegocioDto>();
}
