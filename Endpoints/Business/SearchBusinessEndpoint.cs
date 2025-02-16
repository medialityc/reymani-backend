using System;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using reymani_web_api.Data;
using reymani_web_api.Endpoints.Business.Requests;
using reymani_web_api.Endpoints.Commons.Responses;
using reymani_web_api.Endpoints.Business.Responses;
using reymani_web_api.Services.BlobServices;
using reymani_web_api.Endpoints.Mappers;

using BusinessModel = ReymaniWebApi.Data.Models.Business;

namespace reymani_web_api.Endpoints.Business
{
  public class SearchBusinessEndpoint(AppDbContext dbContext, IBlobService blobService)
      : Endpoint<SearchBusinessRequest, Results<Ok<PaginatedResponse<BusinessResponse>>, ProblemDetails>>
  {
    public override void Configure()
    {
      Get("/business/search");
      Summary(s =>
      {
        s.Summary = "Search for active businesses";
        s.Description = "Searches for active businesses based on the specified criteria.";
      });
      AllowAnonymous();
    }

    public override async Task<Results<Ok<PaginatedResponse<BusinessResponse>>, ProblemDetails>>
        ExecuteAsync(SearchBusinessRequest req, CancellationToken ct)
    {
      var query = dbContext.Businesses.AsNoTracking().AsQueryable();

      // Nuevo: Filtrado para negocios activos
      query = query.Where(b => b.IsActive);

      // Filtrado
      if (req.Ids?.Any() ?? false)
        query = query.Where(b => req.Ids.Contains(b.Id));

      if (req.Names?.Any() ?? false)
        query = query.Where(b => req.Names.Any(n => b.Name.ToLower().Contains(n.ToLower().Trim())));

      if (req.Descriptions?.Any() ?? false)
        query = query.Where(b => b.Description != null && req.Descriptions.Any(d => b.Description.ToLower().Contains(d.ToLower().Trim())));

      if (req.IsAvailable is not null)
        query = query.Where(b => b.IsAvailable == req.IsAvailable);

      // NUEVO: Filtro por municipios
      if (req.MunicipalityIds?.Any() ?? false)
        query = query.Where(b => req.MunicipalityIds.Contains(b.MunicipalityId));

      // NUEVO: Filtro específico por dirección
      if (req.Addresses?.Any() ?? false)
        query = query.Where(b => req.Addresses.Any(a => b.Address.ToLower().Contains(a.ToLower().Trim())));

      if (req.Search is not null)
      {
        var search = req.Search.ToLower().Trim();
        query = query.Where(b =>
            b.Name.ToLower().Contains(search) ||
            (b.Description != null && b.Description.ToLower().Contains(search)) ||
            b.Address.ToLower().Contains(search)
        );
      }

      // Ejecución de la consulta
      var businesses = (await query.ToListAsync(ct)).AsEnumerable();

      // Ordenamiento
      if (!string.IsNullOrEmpty(req.SortBy))
      {
        var propertyInfo = typeof(BusinessModel).GetProperty(req.SortBy);
        if (propertyInfo != null)
        {
          businesses = req.IsDescending ?? false
              ? businesses.OrderByDescending(b => propertyInfo.GetValue(b))
              : businesses.OrderBy(b => propertyInfo.GetValue(b));
        }
      }

      // Paginación
      var totalCount = businesses.Count();
      var data = businesses.Skip(((req.Page ?? 1) - 1) * (req.PageSize ?? 10)).Take(req.PageSize ?? 10);

      var mapper = new BusinessMapper();
      var responseData = await Task.WhenAll(data.Select(async b =>
      {
        var resp = mapper.ToBusinessResponse(b);
        if (!string.IsNullOrEmpty(b.Logo))
          resp.Logo = await blobService.PresignedGetUrl(b.Logo, ct);
        if (!string.IsNullOrEmpty(b.Banner))
          resp.Banner = await blobService.PresignedGetUrl(b.Banner, ct);
        return resp;
      }));

      return TypedResults.Ok(new PaginatedResponse<BusinessResponse>
      {
        Data = responseData,
        Page = req.Page ?? 1,
        PageSize = req.PageSize ?? 10,
        TotalCount = totalCount
      });
    }
  }
}
