using System;
using FluentValidation;

namespace reymani_web_api.Api.Endpoints.Cliente
{
  public class GetClienteByIdRequest
  {
    public Guid ClienteId { get; set; }
  }
}
