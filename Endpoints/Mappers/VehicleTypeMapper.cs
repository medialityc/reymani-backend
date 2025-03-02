using System.Collections.ObjectModel;

using reymani_web_api.Endpoints.Municipalities.Responses;
using reymani_web_api.Endpoints.Provinces.Responses;
using reymani_web_api.Endpoints.ShippingsCost.Responses;
using reymani_web_api.Endpoints.ShippingsCosts.Responses;
using reymani_web_api.Endpoints.Vehicles.Requests;
using reymani_web_api.Endpoints.VehiclesTypes.Requests;
using reymani_web_api.Endpoints.VehiclesTypes.Responses;

using ReymaniWebApi.Data.Models;

namespace reymani_web_api.Endpoints.Mappers;

public class VehicleTypeMapper
{
  public VehicleTypeResponse FromEntity(VehicleType vt, Dictionary<int, List<ReymaniWebApi.Data.Models.ShippingCost>> shippingCostsByVehicleTypeId)
  {
    return new VehicleTypeResponse
    {
      Id = vt.Id,
      Name = vt.Name,
      Logo = vt.Logo ?? string.Empty, // Usar cadena vacía si Logo es nulo
      TotalCapacity = vt.TotalCapacity,
      IsActive = vt.IsActive,
      ShippingCosts = shippingCostsByVehicleTypeId.ContainsKey(vt.Id)
            ? shippingCostsByVehicleTypeId[vt.Id].Select(sc => new ShippingCostVehicleTypeResponse
            {
              Id = sc.Id,
              Cost = sc.Cost,
              MunicipalityId = sc.MunicipalityId,
              MunicipalityName = sc.Municipality?.Name ?? string.Empty // Usar cadena vacía si Name es nulo
            }).ToList()
            : new List<ShippingCostVehicleTypeResponse>() // Usar lista vacía si no hay costos de envío
    };
  }

  public VehicleTypeResponse FromEntity(VehicleType vt)
  {
    return new VehicleTypeResponse
    {
      Id = vt.Id,
      Name = vt.Name,
      Logo = vt.Logo ?? string.Empty, // Usar cadena vacía si Logo es nulo
      TotalCapacity = vt.TotalCapacity,
      IsActive = vt.IsActive
    };
  }

  public VehicleTypeResponse FromEntity(VehicleType vt, List<ReymaniWebApi.Data.Models.ShippingCost> shippingCosts)
  {
    return new VehicleTypeResponse
    {
      Id = vt.Id,
      Name = vt.Name,
      Logo = vt.Logo ?? string.Empty, // Usar cadena vacía si Logo es nulo
      TotalCapacity = vt.TotalCapacity,
      IsActive = vt.IsActive,
      ShippingCosts = shippingCosts
            .Select(sc => new ShippingCostVehicleTypeResponse
            {
              Id = sc.Id,
              Cost = sc.Cost,
              MunicipalityId = sc.MunicipalityId,
              MunicipalityName = sc.Municipality?.Name ?? string.Empty // Usar cadena vacía si Name es nulo
            })
            .ToList()
    };
  }

  public VehicleType ToEntity(CreateVehicleTypeRequest req)
  {
    return new VehicleType
    {
      Name = req.Name,
      IsActive = req.IsActive,
      Logo = "",
      TotalCapacity = req.TotalCapacity,
    };
  }
}
