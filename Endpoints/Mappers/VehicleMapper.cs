using reymani_web_api.Endpoints.Municipalities.Responses;
using reymani_web_api.Endpoints.Provinces.Requests;
using reymani_web_api.Endpoints.Provinces.Responses;
using reymani_web_api.Endpoints.Vehicles.Requests;
using reymani_web_api.Endpoints.Vehicles.Response;

using ReymaniWebApi.Data.Models;

namespace reymani_web_api.Endpoints.Mappers;

public class VehicleMapper
{
  public VehicleResponse FromEntity(Vehicle e)
  {
    return new VehicleResponse
    {
      Id = e.Id,
      Picture = e.Picture,
      UserId = e.UserId,
      Name = e.Name,
      VehicleTypeName = e.VehicleType?.Name ?? "",
      Description = e.Description,
      VehicleTypeId = e.VehicleTypeId,
      IsActive = e.IsActive,
      IsAvailable = e.IsAvailable,
    };
  }

  //Crear vehiculo mensajero
  public Vehicle ToEntity(CreateVehicleCourierRequest req)
  {
    return new Vehicle
    {
      UserId = 0,
      VehicleTypeId = req.VehicleTypeId,
      Description = req.Description?? "",
      IsActive = req.IsActive,
      IsAvailable = req.IsAvailable,
      Picture = "",
      Name = req.Name,
    };
  }

  //Creat vehiculo administrador
  public Vehicle ToEntityAdmin(CreateVehicleAdminRequest req)
  {
    return new Vehicle
    {
      UserId = req.UserId,
      VehicleTypeId = req.VehicleTypeId,
      Description = req.Description ?? "",
      IsActive = req.IsActive,
      IsAvailable = req.IsAvailable,
      Picture = "",
      Name = req.Name,
    };
  }
}
