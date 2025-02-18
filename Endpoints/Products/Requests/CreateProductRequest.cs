using System;

using reymani_web_api.Data.Models;

namespace reymani_web_api.Endpoints.Products.Requests;

public class CreateProductRequest
{
  public required string Name { get; set; }
  public required string? Description { get; set; }
  public required int BusinessId { get; set; }
  public required bool IsAvailable { get; set; }
  public required bool IsActive { get; set; }
  public required List<IFormFile>? Images { get; set; }
  public required decimal Price { get; set; }
  public required decimal? DiscountPrice { get; set; }
  public required int CategoryId { get; set; }
  public required Capacity Capacity { get; set; }
}