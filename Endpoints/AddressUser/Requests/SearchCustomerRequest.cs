﻿namespace reymani_web_api.Endpoints.AddressUser.Requests;

public class SearchCustomerRequest
{
  public int[]? Ids { get; set; }
  public string[]? Names { get; set; }
  public string[]? Notes { get; set; }
  public string[]? Address { get; set; }
  public string[]? NameMunicipality { get; set; }
  public string[]? NameProvince { get; set; }
  public string? Search { get; set; }

  // Ordenamiento y paginación
  public string? SortBy { get; set; } = "FirstName";
  public bool? IsDescending { get; set; } = false;
  public int? Page { get; set; } = 1;
  public int? PageSize { get; set; } = 10;
}
