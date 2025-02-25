using ReymaniWebApi.Data.Models;

namespace reymani_web_api.Endpoints.AddressUser.Responses;

public class UserAddressResponse
{
  public int Id { get; set; }
  public required string Name { get; set; }
  public required string Address { get; set; }
  public required int MunicipalityId { get; set; }
  public required string MunicipalityName { get; set; }
  public required int ProvinceId { get; set; }
  public required string ProvinceName { get; set; }
}
