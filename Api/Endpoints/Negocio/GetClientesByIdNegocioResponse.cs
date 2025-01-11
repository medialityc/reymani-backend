using System;
using reymani_web_api.Application.DTOs;

namespace reymani_web_api.Api.Endpoints.Negocio;

public class GetClientesByIdNegocioResponse
{
  public required IEnumerable<ClienteDto> Clientes { get; set; }
}
