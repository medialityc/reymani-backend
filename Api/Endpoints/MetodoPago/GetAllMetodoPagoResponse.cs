
namespace reymani_web_api.Api.Endpoints.MetodoPago;

public class GetAllMetodoPagoResponse
{
  public required List<Domain.Entities.MetodoPago> MetodosPago { get; set; }
}