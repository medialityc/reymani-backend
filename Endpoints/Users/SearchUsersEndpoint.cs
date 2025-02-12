using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using reymani_web_api.Data;
using reymani_web_api.Endpoints.Users.Requests;
using reymani_web_api.Endpoints.Commons.Responses;
using reymani_web_api.Endpoints.Users.Responses;
using ReymaniWebApi.Data.Models;
using reymani_web_api.Services.BlobServices;

namespace reymani_web_api.Endpoints.Users
{
  public class SearchUsersEndpoint(AppDbContext dbContext, IBlobService blobService)
      : Endpoint<SearchUsersRequest, Results<Ok<PaginatedResponse<UserResponse>>, ProblemDetails>>
  {
    public override void Configure()
    {
      Get("/users/search");
      Roles("SystemAdmin");
      Summary(s =>
      {
        s.Summary = "Search users";
        s.Description = "Searches for users based on the specified criteria.";
      });
    }

    public override async Task<Results<Ok<PaginatedResponse<UserResponse>>, ProblemDetails>>
        ExecuteAsync(SearchUsersRequest req, CancellationToken ct)
    {
      var query = dbContext.Users.AsNoTracking().AsQueryable();

      // Filtrado
      if (req.Ids?.Any() ?? false)
        query = query.Where(u => req.Ids.Contains(u.Id));

      if (req.IsActive is not null)
        query = query.Where(u => u.IsActive == req.IsActive);

      if (req.IsConfirmed is not null)
        query = query.Where(u => u.IsConfirmed == req.IsConfirmed);

      if (req.FirstNames?.Any() ?? false)
        query = query.Where(u => req.FirstNames.Any(n => u.FirstName.ToLower().Contains(n.ToLower().Trim())));

      if (req.LastNames?.Any() ?? false)
        query = query.Where(u => req.LastNames.Any(n => u.LastName.ToLower().Contains(n.ToLower().Trim())));

      if (req.Emails?.Any() ?? false)
        query = query.Where(u => req.Emails.Any(e => u.Email.ToLower().Contains(e.ToLower().Trim())));

      if (req.Search is not null)
      {
        var search = req.Search.ToLower().Trim();
        query = query.Where(u =>
            u.FirstName.ToLower().Contains(search) ||
            u.LastName.ToLower().Contains(search) ||
            u.Email.ToLower().Contains(search) ||
            (u.Phone != null && u.Phone.ToLower().Contains(search))
        );
      }

      // Ejecución de la consulta
      var users = (await query.ToListAsync(ct)).AsEnumerable();

      // Ordenamiento
      if (!string.IsNullOrEmpty(req.SortBy))
      {
        var propertyInfo = typeof(User).GetProperty(req.SortBy);
        if (propertyInfo != null)
        {
          users = req.IsDescending ?? false
              ? users.OrderByDescending(u => propertyInfo.GetValue(u))
              : users.OrderBy(u => propertyInfo.GetValue(u));
        }
      }

      // Paginación
      var totalCount = users.Count();
      var data = users.Skip(((req.Page ?? 1) - 1) * (req.PageSize ?? 10)).Take(req.PageSize ?? 10);

      // Mapeo a UserResponse (se asume que dicho tipo define estas propiedades)
      var responseData = await Task.WhenAll(data.Select(async u => new UserResponse
      {
        Id = u.Id,
        ProfilePicture = u.ProfilePicture != null ? await blobService.PresignedGetUrl(u.ProfilePicture, ct) : null,
        FirstName = u.FirstName,
        LastName = u.LastName,
        Email = u.Email,
        Phone = u.Phone,
        IsActive = u.IsActive,
        IsConfirmed = u.IsConfirmed,
        Role = u.Role
      }));

      return TypedResults.Ok(new PaginatedResponse<UserResponse>
      {
        Data = responseData,
        Page = req.Page ?? 1,
        PageSize = req.PageSize ?? 10,
        TotalCount = totalCount
      });
    }
  }
}
