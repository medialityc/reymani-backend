using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

using reymani_web_api.Data;
using reymani_web_api.Endpoints.AddressUser.Requests;
using reymani_web_api.Endpoints.AddressUser.Responses;
using reymani_web_api.Endpoints.Commons.Responses;
using reymani_web_api.Endpoints.Mappers;

using ReymaniWebApi.Data.Models;

namespace reymani_web_api.Endpoints.AddressUser;

public class SearchSystemAdminEndpoint : Endpoint<SearchCustomerRequest, Results<Ok<PaginatedResponse<UserAddressResponse>>, ProblemDetails>>
{
  private readonly AppDbContext _dbContext;

  public SearchSystemAdminEndpoint(AppDbContext dbContext)
  {
    _dbContext = dbContext;
  }

  public override void Configure()
  {
    Get("/userAddress/admin/search");
    Summary(s =>
    {
      s.Summary = "Search userAddress";
      s.Description = "Search for userAddress by name or ID with filtering, sorting, and pagination.";
    });
    Roles("SystemAdmin");
  }

  public override async Task<Results<Ok<PaginatedResponse<UserAddressResponse>>, ProblemDetails>> ExecuteAsync(SearchCustomerRequest req, CancellationToken ct)
  {
    var query = _dbContext.UserAddresses
      .AsNoTracking()
      .Include(p => p.Municipality)
      .Include(p=> p.Municipality.Province)
      .AsQueryable();

    //Filtrado
    if (req.Ids?.Any() ?? false)
      query = query.Where(pc => req.Ids.Contains(pc.Id));

    if (req.Names?.Any() ?? false)
      query = query.Where(pc => req.Names.Any(n => pc.Name.ToLower().Contains(n.ToLower().Trim())));

    if (req.Notes?.Any() ?? false)
      query = query.Where(pc => req.Notes.Contains(pc.Notes));

    if (req.Address?.Any() ?? false)
      query = query.Where(pc => req.Address.Contains(pc.Address));

    if (req.IdMunicipalities?.Any() ?? false)
      query = query.Where(pc => req.IdMunicipalities.Contains(pc.MunicipalityId));


    if (req.Search is not null)
    {
      var search = req.Search.ToLower().Trim();
      query = query.Where(pc => pc.Name.ToLower().Contains(search) || pc.Address.ToLower().Contains(search) || pc.Notes.ToLower().Contains(search));
    }

    // Ejecución de la consulta
    var categories = (await query.ToListAsync(ct)).AsEnumerable();

    // Ordenamiento
    if (!string.IsNullOrEmpty(req.SortBy))
    {
      var propertyInfo = typeof(UserAddress).GetProperty(req.SortBy);
      if (propertyInfo != null)
      {
        query = req.IsDescending ?? false
        ? query.OrderByDescending(u => propertyInfo.GetValue(u))
            : query.OrderBy(u => propertyInfo.GetValue(u));
      }
    }

    // Paginación
    var totalCount = await query.CountAsync(ct);
    var data = await query
           .Skip(((req.Page ?? 1) - 1) * (req.PageSize ?? 10))
           .Take(req.PageSize ?? 10)
           .ToListAsync(ct);

    // Mapeo de respuesta
    var mapper = new UserAddressMapper();
    var responseData = data.Select(u => mapper.FromEntity(u)).ToList();

    return TypedResults.Ok(new PaginatedResponse<UserAddressResponse>
    {
      Data = responseData,
      Page = req.Page ?? 1,
      PageSize = req.PageSize ?? 10,
      TotalCount = totalCount
    });
  }
}
