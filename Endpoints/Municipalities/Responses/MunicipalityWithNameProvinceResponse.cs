using ReymaniWebApi.Data.Models;

namespace reymani_web_api.Endpoints.Municipalities.Responses;

public class MunicipalityWithNameProvinceResponse
{
  public int Id { get; set; }
  public required string Name { get; set; }
  public required string ProvinceName {  get; set; }
  public required int ProvindeId {  get; set; }
}
