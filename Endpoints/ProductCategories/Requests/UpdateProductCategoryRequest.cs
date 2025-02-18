using System;

using Microsoft.AspNetCore.Http;

namespace reymani_web_api.Endpoints.ProductCategories.Requests;

public class UpdateProductCategoryRequest
{
  public required int Id { get; set; }
  public required string Name { get; set; }
  public required IFormFile? Logo { get; set; }
  public required bool IsActive { get; set; }
}