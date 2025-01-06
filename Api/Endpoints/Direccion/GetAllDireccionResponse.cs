using System;
using System.Collections.Generic;
using reymani_web_api.Domain.Entities;

namespace reymani_web_api.Api.Endpoints.Direccion;

public class GetAllDireccionResponse
{
  public List<Domain.Entities.Direccion> Direcciones { get; set; } = new List<Domain.Entities.Direccion>();
}
