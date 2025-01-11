using System;
using reymani_web_api.Application.DTOs;

namespace reymani_web_api.Api.Endpoints.Cliente
{
  public class GetNegociosByIdClienteResponse
  {
    public required IEnumerable<NegocioDto> Negocios { get; set; }
  }
}
