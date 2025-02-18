namespace reymani_web_api.Endpoints.Municipalities.Requests;

public class CreateMunicipalityRequest
{
  public required string Name { get; set; }
  public required int ProvinceId {  get; set; }
}
