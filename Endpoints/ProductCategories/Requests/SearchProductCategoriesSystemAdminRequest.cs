using System;

namespace reymani_web_api.Endpoints.ProductCategories.Requests;

public class SearchProductCategoriesSystemAdminRequest
{
  // Filtros
  public int[]? Ids { get; set; }
  public string[]? Names { get; set; }
  public bool? IsActive { get; set; }
  public string? Search { get; set; }
  // Ordenamiento y paginación
  public string? SortBy { get; set; } = "Name";
  public bool? IsDescending { get; set; } = false;
  public int? Page { get; set; } = 1;
  public int? PageSize { get; set; } = 10;

}
