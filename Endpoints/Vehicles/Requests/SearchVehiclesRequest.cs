namespace reymani_web_api.Endpoints.Vehicles.Requests;

public class SearchVehiclesRequest
{
  public required string[]? Names { get; set; }
  public required string[]? Descriptions { get; set; }
  public int[]? Ids { get; set; }
  public int[]? UserIds { get; set; }
  public required bool? IsAvailable { get; set; }
  public required int[]? TypesVehicle { get; set; }
  public required string? Search { get; set; }

  // Ordenamiento y paginación
  public string? SortBy { get; set; } = "Name";
  public bool? IsDescending { get; set; } = false;
  public int? Page { get; set; } = 1;
  public int? PageSize { get; set; } = 10;
}
