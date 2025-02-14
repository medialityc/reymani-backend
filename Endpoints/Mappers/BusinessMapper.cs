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

    public BusinessModel ToEntity(UpdateBusinessRequest r, BusinessModel existing)
    {
      existing.Name = r.Name;
      existing.Description = r.Description;
      existing.UserId = r.UserId;
      existing.Address = r.Address;
      existing.MunicipalityId = r.MunicipalityId;
      existing.IsAvailable = r.IsAvailable;
      existing.IsActive = r.IsActive;
      return existing;
    }

    // Nuevo método para mapear UpdateMyBusinessRequest
    public BusinessModel ToEntity(UpdateMyBusinessRequest req, BusinessModel existing)
    {
      existing.Name = req.Name;
      existing.Description = req.Description;
      existing.Address = req.Address;
      existing.MunicipalityId = req.MunicipalityId;
      existing.IsAvailable = req.IsAvailable;
      return existing;
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
        MunicipalityId = business.Municipality?.Id ?? 0,
        MunicipalityName = business.Municipality?.Name ?? string.Empty,
        ProvinceId = business.Municipality?.ProvinceId ?? 0,
        ProvinceName = business.Municipality?.Province?.Name ?? string.Empty,
        IsAvailable = business.IsAvailable,
        Logo = string.Empty,
        Banner = string.Empty
      };
    }
  }
}
