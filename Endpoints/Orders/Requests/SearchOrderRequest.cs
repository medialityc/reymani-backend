using reymani_web_api.Data.Models;

namespace reymani_web_api.Endpoints.Orders.Requests;

public class SearchOrderRequest
{
  public int[]? Ids { get; set; }
  public OrderStatus[]? Status { get; set; }
  public int[]? CourierId { get; set; }
  public int[]? CustomerId { get; set; }

  // Ordenamiento y paginación
  public string? SortBy { get; set; } = "Status";
  public bool? IsDescending { get; set; } = false;
  public int? Page { get; set; } = 1;
  public int? PageSize { get; set; } = 10;
}
