using System;

namespace reymani_web_api.Endpoints.Business.Requests;

public class SearchBusinessRequest
{
  // Filtros
  public int[]? Ids { get; set; }
  public string[]? Names { get; set; }
  public string[]? Descriptions { get; set; }
  // NUEVOS FILTROS
  public int[]? MunicipalityIds { get; set; }
  public string[]? Addresses { get; set; }
  public bool? IsAvailable { get; set; }
  public string? Search { get; set; }
  // Ordenamiento y paginación
  public string? SortBy { get; set; } = "Name";
  public bool? IsDescending { get; set; } = false;
  public int? Page { get; set; } = 1;
  public int? PageSize { get; set; } = 10;
}
