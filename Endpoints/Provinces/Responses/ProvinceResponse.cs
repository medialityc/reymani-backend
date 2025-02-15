using System;

using reymani_web_api.Data.Models;
using reymani_web_api.Endpoints.Municipalities.Responses;

using ReymaniWebApi.Data.Models;

namespace reymani_web_api.Endpoints.Users.Responses;

public class ProvinceResponse
{
  public int Id { get; set; }
  public required string Name { get; set; }
  public ICollection<MunicipalityResponse>? Municipalities { get; set; }
}
