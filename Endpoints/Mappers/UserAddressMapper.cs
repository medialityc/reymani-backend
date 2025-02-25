using reymani_web_api.Endpoints.AddressUser.Requests;
using reymani_web_api.Endpoints.AddressUser.Responses;
using reymani_web_api.Endpoints.Municipalities.Responses;
using reymani_web_api.Endpoints.Provinces.Requests;
using reymani_web_api.Endpoints.Provinces.Responses;
using reymani_web_api.Endpoints.Users.Requests;

using ReymaniWebApi.Data.Models;

namespace reymani_web_api.Endpoints.Mappers;

public class UserAddressMapper
{

  //Crear una respuesta apartir de la entidad
  public UserAddressResponse FromEntity(UserAddress e)
  {
    return new UserAddressResponse
    {
      Id = e.Id,
      Address = e.Address,
      Name = e.Name,
      ProvinceId = e.Municipality?.Province?.Id ?? 0,
      ProvinceName = e.Municipality?.Province?.Name ?? "",
      MunicipalityId = e.MunicipalityId,
      MunicipalityName = e.Municipality?.Name ?? ""
    };
  }


  //Crear la entidad apartir de la request de crear
  public UserAddress ToEntity(CreateCustomerRequest r) => new()
  {
    Address = string.Empty,
    MunicipalityId = r.MunicipalityId,
    Name = r.Name,
    Notes = r.Notes,
    UserId = r.UserId,
    IsActive = r.IsActive
  };
}
