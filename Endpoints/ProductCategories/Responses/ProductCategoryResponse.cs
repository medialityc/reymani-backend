using System;

namespace reymani_web_api.Endpoints.ProductCategories.Responses;

public class ProductCategoryResponse
{
  public required int Id { get; set; }
  public required string Name { get; set; }
  public required string? Logo { get; set; }
  public required bool IsActive { get; set; }

}