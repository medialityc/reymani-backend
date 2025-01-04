
namespace reymani_web_api.Application.DTOs;

public class NegocioDto
{
  public Guid IdNegocio { get; set; }
  public required string Nombre { get; set; }
  public string? Descripcion { get; set; }
  public bool EntregaDomicilio { get; set; }
  public string? URLImagenPrincipal { get; set; }
  public string? URLImagenLogo { get; set; }
  public string? URLImagenBanner { get; set; }

}
