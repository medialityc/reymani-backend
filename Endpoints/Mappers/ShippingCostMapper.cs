
using reymani_web_api.Endpoints.ShippingCost.Request;
using reymani_web_api.Endpoints.ShippingsCost.Responses;

using ReymaniWebApi.Data.Models;

namespace reymani_web_api.Endpoints.Mappers;

public class ShippingCostMapper
{
  public ShippingCostResponse FromEntity(ReymaniWebApi.Data.Models.ShippingCost e)
  {
    return new ShippingCostResponse
    {
      Id = e.Id,
      Cost=e.Cost,
      MunicipalityId = e.MunicipalityId,
      MunicipalityName = e.Municipality?.Name ?? "",
      VehicleTypeId = e.VehicleTypeId,
      VehicleName = e.VehicleType?.Name ?? ""
    };
  }

  public ReymaniWebApi.Data.Models.ShippingCost ToEntity(CreateShippingCostRequest req)
  {
    return new ReymaniWebApi.Data.Models.ShippingCost
    {
      Cost=req.Cost,
      MunicipalityId = req.MunicipalityId,
      VehicleTypeId = req.VehicleTypeId
    };
  }
}
