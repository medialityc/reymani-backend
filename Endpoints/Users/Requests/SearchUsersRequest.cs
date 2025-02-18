using System;

namespace reymani_web_api.Endpoints.Users.Requests;

public class SearchUsersRequest
{
  // Filtros
  public int[]? Ids { get; set; }
  public string[]? FirstNames { get; set; }
  public string[]? LastNames { get; set; }
  public string[]? Emails { get; set; }
  public bool? IsActive { get; set; }
  public bool? IsConfirmed { get; set; }
  public string? Search { get; set; }
  // Ordenamiento y paginación
  public string? SortBy { get; set; } = "FirstName";
  public bool? IsDescending { get; set; } = false;
  public int? Page { get; set; } = 1;
  public int? PageSize { get; set; } = 10;
}