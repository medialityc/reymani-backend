using System;

using reymani_web_api.Data.Models;

namespace reymani_web_api.Endpoints.Products.Requests;

public class SearchProductsSystemAdminRequest
{
public int[]? Ids { get; set; }
    public string[]? Names { get; set; }
    public string[]? Description { get; set; }
    public string? Search { get; set; }
    public bool? IsAvailable { get; set; }
    public decimal? PriceMin { get; set; }
    public decimal? PriceMax { get; set; }
    public bool? HasDiscount { get; set; }
    public decimal? RatingMin { get; set; }
    public bool? IsActive {get; set;}
    public int? CategoryId { get; set; }  // Filtrar por categoría dada
    public int? BusinessId { get; set; }  // Filtrar por negocio dado
    public Capacity? Capacity { get; set; }    // Filtrar por capacidad mínima

    // Ordenamiento y paginación
    public string? SortBy { get; set; } = "Name";
    public bool? IsDescending { get; set; } = false;
    public int? Page { get; set; } = 1;
    public int? PageSize { get; set; } = 10;
}
