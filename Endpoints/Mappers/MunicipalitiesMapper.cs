using reymani_web_api.Endpoints.Municipalities.Requests;
using reymani_web_api.Endpoints.Municipalities.Responses;
using reymani_web_api.Endpoints.Provinces.Requests;
using reymani_web_api.Endpoints.Provinces.Responses;
using ReymaniWebApi.Data.Models;

namespace reymani_web_api.Endpoints.Mappers;

public class MunicipalitiesMapper
{
  public MunicipalityResponse FromEntity(Municipality e)
  {
    return new MunicipalityResponse
    {
      Id = e.Id,
      Name = e.Name,
      ProvinceName = e.Province?.Name ?? "",
      ProvindeId = e.ProvinceId,
    };

    
    }
  public Municipality ToEntity(CreateMunicipalityRequest req)
  {
    return new Municipality
    {
      Name = req.Name,
      ProvinceId = req.ProvinceId,
      CreatedAt = DateTime.UtcNow,
      UpdatedAt = DateTime.UtcNow
    };
  }
}
