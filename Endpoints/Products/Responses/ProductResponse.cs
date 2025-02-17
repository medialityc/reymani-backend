using System;
using System.Collections.Generic;

namespace reymani_web_api.Endpoints.Products.Responses;

public class ProductResponse
{
  public required int Id { get; set; }
  public required string Name { get; set; }
  public required string? Description { get; set; }
  public required int BusinessId { get; set; }
  public required string BusinessName { get; set; }
  public required bool IsAvailable { get; set; }
  public required bool IsActive { get; set; }
  public required List<string>? Images { get; set; }
  public required decimal Price { get; set; }
  public required decimal? DiscountPrice { get; set; }
  public required int CategoryId { get; set; }
  public required string CategoryName { get; set; }
  public required int Capacity { get; set; }
}
