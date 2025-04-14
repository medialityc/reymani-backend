﻿using reymani_web_api.Endpoints.AddressUser.Requests;
using reymani_web_api.Endpoints.AddressUser.Responses;


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
      MunicipalityName = e.Municipality?.Name ?? "",
    };
  }


  //Crear la entidad apartir de la request de crear
  public UserAddress ToEntity(CreateCustomerRequest r, int userId) => new()
  {
    Address = r.Address,
    MunicipalityId = r.MunicipalityId,
    Name = r.Name,
    Notes = r.Notes,
    UserId = userId,
    IsActive = true,
  };
}
