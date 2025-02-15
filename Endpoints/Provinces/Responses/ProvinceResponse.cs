using System;

using reymani_web_api.Data.Models;
using reymani_web_api.Endpoints.Municipalities.Responses;


namespace reymani_web_api.Endpoints.Provinces.Responses;

public class ProvinceResponse
{
  public int Id { get; set; }
  public required string Name { get; set; }
  public ICollection<MunicipalityResponse>? Municipalities { get; set; }
}
