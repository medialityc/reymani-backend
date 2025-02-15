using reymani_web_api.Endpoints.Municipalities.Responses;
using reymani_web_api.Endpoints.Users.Responses;
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
        Name = m.Name
      }).ToList()
    };
  }

}
