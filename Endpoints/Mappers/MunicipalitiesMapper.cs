using reymani_web_api.Endpoints.Municipalities.Requests;
using reymani_web_api.Endpoints.Municipalities.Responses;
using reymani_web_api.Endpoints.Provinces.Requests;
using reymani_web_api.Endpoints.Provinces.Responses;
using ReymaniWebApi.Data.Models;

namespace reymani_web_api.Endpoints.Mappers;

public class MunicipalitiesMapper
{
  public MunicipalityWithNameProvinceResponse FromEntity(Municipality e)
  {
    return new MunicipalityWithNameProvinceResponse
    {
      Id = e.Id,
      Name = e.Name,
      ProvinceName = e.Province?.Name ?? "",
      ProvindeId = e.ProvinceId,
    };
  }

  public MunicipalityResponse FromEntity_NoNameProvince(Municipality e)
  {
    return new MunicipalityResponse
    {
      Id = e.Id,
      Name = e.Name,
    };
  }

  //Metodo para crear una instancia de Municipio
  public Municipality ToEntity(CreateMunicipalityRequest req)
  {
    return new Municipality
    {
      Name = req.Name,
      ProvinceId = req.ProvinceId,
    };
  }


  //Metodo para editar una instancia
  public Municipality ToEntity(UpdateMunicipalityRequest req,Municipality municipality,Province province)
  {
    municipality.Name= req.Name;
    municipality.ProvinceId= req.ProvinceId;
    municipality.Province= province;
    return municipality;
  }
}
