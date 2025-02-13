using System;

using reymani_web_api.Endpoints.Business.Requests;
using reymani_web_api.Endpoints.Business.Responses;

using BusinessModel = ReymaniWebApi.Data.Models.Business;

namespace reymani_web_api.Endpoints.Mappers
{
  public class BusinessMapper
  {
    public BusinessModel ToEntity(CreateBusinessRequest request)
    {
      return new BusinessModel
      {
        Name = request.Name,
        Logo = string.Empty,
        Banner = string.Empty,
        Description = request.Description,
        UserId = request.UserId,
        Address = request.Address,
        MunicipalityId = request.MunicipalityId,
        IsAvailable = request.IsAvailable,
        IsActive = request.IsActive
      };
    }

    public BusinessSystemAdminResponse FromEntity(BusinessModel business)
    {
      // Se aplican comprobaciones para evitar referencias nulas.
      var userFullName = (business.User?.FirstName ?? "") + " " + (business.User?.LastName ?? "");
      var municipalityName = business.Municipality?.Name ?? "";
      var provinceId = business.Municipality?.ProvinceId ?? 0;
      var provinceName = business.Municipality?.Province?.Name ?? "";

      return new BusinessSystemAdminResponse
      {
        Id = business.Id,
        Name = business.Name,
        Description = business.Description,
        UserId = business.UserId,
        UserFullName = userFullName.Trim(),
        Address = business.Address,
        MunicipalityId = business.Municipality?.Id ?? business.MunicipalityId,
        MunicipalityName = municipalityName,
        IsAvailable = business.IsAvailable,
        IsActive = business.IsActive,
        Logo = string.Empty,
        Banner = string.Empty,
        ProvinceId = provinceId,
        ProvinceName = provinceName
      };
    }

    public BusinessResponse ToBusinessResponse(BusinessModel business)
    {
      return new BusinessResponse
      {
        Id = business.Id,
        Name = business.Name,
        Description = business.Description,
        Address = business.Address,
        MunicipalityId = business.Municipality?.Id ?? business.MunicipalityId,
        MunicipalityName = business.Municipality?.Name ?? "",
        ProvinceId = business.Municipality?.ProvinceId ?? 0,
        ProvinceName = business.Municipality?.Province?.Name ?? "",
        IsAvailable = business.IsAvailable,
        Logo = string.Empty,
        Banner = string.Empty
      };
    }
  }
}
