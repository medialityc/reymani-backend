
using System.Xml.Linq;

using reymani_web_api.Endpoints.Municipalities.Responses;
using reymani_web_api.Endpoints.Provinces.Requests;
using reymani_web_api.Endpoints.Provinces.Responses;
using ReymaniWebApi.Data.Models;

namespace reymani_web_api.Endpoints.Mappers;

public class ProvinceMapper
{
  public ProvinceResponse FromEntity(Province e)
  {
    return new ProvinceResponse
    {
      Id = e.Id,
      Name = e.Name,
      Municipalities = e.Municipalities?.Select(m => new MunicipalityResponse
      {
        Id = m.Id,
        Name = m.Name,
        ProvinceName = e.Name,
        ProvindeId = m.Id,
      }).ToList()
    };
  }

  public Province ToEntity(CreateProvinceRequest req)
  {
    return new Province
    {
      Name = req.Name,
      CreatedAt = DateTime.UtcNow,
      UpdatedAt = DateTime.UtcNow
    };
  }
}
