using System;

namespace reymani_web_api.Api.Endpoints.CategoriaNegocio
{
  public class AssignCategoriaNegocioToNegocioRequest
  {
    public Guid NegocioId { get; set; }
    public List<Guid> CategoriaNegocioIds { get; set; } = new List<Guid>();
  }
}
