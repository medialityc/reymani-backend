namespace reymani_web_api.Api.Endpoints.Telefono;

public class GetAllTelefonoResponse
{
  public List<Domain.Entities.Telefono> Telefonos { get; set; } = new List<Domain.Entities.Telefono>();
}
