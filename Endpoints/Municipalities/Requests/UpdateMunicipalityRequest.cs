namespace reymani_web_api.Endpoints.Municipalities.Requests;

public class UpdateMunicipalityRequest
{
  public required int Id { get; set; }
  public required string Name { get; set; }
  public required int ProvinceId {  get; set; }
}
