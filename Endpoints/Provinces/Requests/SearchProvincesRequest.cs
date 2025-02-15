namespace reymani_web_api.Endpoints.Provinces.Requests;

public class SearchProvincesRequest
{
  public int? Id { get; set; }
  public string? Name { get; set; }

  // Ordenamiento y paginación
  public string? SortBy { get; set; } = "FirstName";
  public bool? IsDescending { get; set; } = false;
  public int? Page { get; set; } = 1;
  public int? PageSize { get; set; } = 10;
}
