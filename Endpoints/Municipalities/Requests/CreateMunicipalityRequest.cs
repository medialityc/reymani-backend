namespace reymani_web_api.Endpoints.Municipalities.Requests;

public class CreateMunicipalitieRequest
{
  public required string Name { get; set; }
  public required int ProvinceId {  get; set; }
}
