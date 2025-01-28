using System;
using FluentValidation;

namespace reymani_web_api.Api.Endpoints.Usuario
{
  public class GetUsuarioByIdRequest
  {
    public Guid UsuarioId { get; set; }
  }
}
