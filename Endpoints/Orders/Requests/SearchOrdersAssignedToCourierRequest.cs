namespace reymani_web_api.Endpoints.Orders.Requests;

public class SearchOrdersAssignedToCourierRequest
{
  //Parametros de filtrado

  // Ordenamiento y paginación
  public string? SortBy { get; set; } = "Status";
  public bool? IsDescending { get; set; } = false;
  public int? Page { get; set; } = 1;
  public int? PageSize { get; set; } = 10;
}
