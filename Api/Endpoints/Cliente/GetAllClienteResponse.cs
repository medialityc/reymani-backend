using System;
using reymani_web_api.Application.DTOs;

namespace reymani_web_api.Api.Endpoints.Cliente;

public class GetAllClienteResponse
{
  public List<ClienteDto> Clientes { get; set; } = new List<ClienteDto>();
}