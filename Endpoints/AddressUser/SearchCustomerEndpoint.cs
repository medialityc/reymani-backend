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

public class SearchCustomerEndpoint : Endpoint<SearchCustomerRequest, Results<Ok<PaginatedResponse<UserAddressResponse>>, ProblemDetails>>
{
  private readonly AppDbContext _dbContext;

  public SearchCustomerEndpoint(AppDbContext dbContext)
  {
    _dbContext = dbContext;
  }

  public override void Configure()
  {
    Get("/userAddress/search");
    Summary(s =>
    {
      s.Summary = "Search userAddress";
      s.Description = "Search for not active userAddress by name or ID with filtering, sorting, and pagination.";
    });
    Roles("Customer");
  }

  public override async Task<Results<Ok<PaginatedResponse<UserAddressResponse>>, ProblemDetails>> ExecuteAsync(SearchCustomerRequest req, CancellationToken ct)
  {
    var query = _dbContext.UserAddresses
        .Where(p => p.IsActive == true)
        .AsNoTracking()
        .Include(p => p.Municipality)
        .Include(p => p.Municipality.Province)
        .AsQueryable();

    // Filtrado
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

    // Ejecutar la consulta sin ordenamiento (traer datos a memoria)
    var userAddresses = await query.ToListAsync(ct);

    // Ordenamiento del lado del cliente (en memoria)
    if (!string.IsNullOrEmpty(req.SortBy))
    {
      var propertyInfo = typeof(UserAddress).GetProperty(req.SortBy);
      if (propertyInfo != null)
      {
        userAddresses = req.IsDescending ?? false
            ? userAddresses.OrderByDescending(u => propertyInfo.GetValue(u)).ToList() // Orden descendente
            : userAddresses.OrderBy(u => propertyInfo.GetValue(u)).ToList(); // Orden ascendente
      }
    }

    // Paginación en memoria
    var totalCount = userAddresses.Count;
    var data = userAddresses
        .Skip(((req.Page ?? 1) - 1) * (req.PageSize ?? 10))
        .Take(req.PageSize ?? 10)
        .ToList();

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
