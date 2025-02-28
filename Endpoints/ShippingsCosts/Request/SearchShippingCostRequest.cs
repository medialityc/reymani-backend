namespace reymani_web_api.Endpoints.ShippingCost.Request;

public class SearchShippingCostRequest
{
  public int[]? Ids { get; set; }
  public int[]? MunicipalitiesIds { get; set; }
  public int[]? VehicleTypesIds { get; set; }
  public decimal? CostMin { get; set; }
  public decimal? CostMax { get; set; }

  // Ordenamiento y paginación
  public string? SortBy { get; set; } = "Cost";
  public bool? IsDescending { get; set; } = false;
  public int? Page { get; set; } = 1;
  public int? PageSize { get; set; } = 10;
}
