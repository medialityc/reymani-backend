namespace reymani_web_api.Endpoints.VehiclesTypes.Requests;

public class SearchVehicleTypeCourierRequest
{
  public required int[]? Ids { get; set; }
  public required string[]? Names { get; set; }
  public required int? CapacityMax { get; set; }
  public required int? CapacityMin { get; set; }
  public required string? Search { get; set; }

  // Ordenamiento y paginación
  public string? SortBy { get; set; } = "Name";
  public bool? IsDescending { get; set; } = false;
  public int? Page { get; set; } = 1;
  public int? PageSize { get; set; } = 10;
}
