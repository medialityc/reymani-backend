using ReymaniWebApi.Data.Models;

namespace reymani_web_api.Endpoints.Municipalities.Responses;

public class MunicipalityResponse
{
  public int Id { get; set; }
  public required string Name { get; set; }
}
