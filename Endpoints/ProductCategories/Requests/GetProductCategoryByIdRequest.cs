using System;

namespace reymani_web_api.Endpoints.ProductCategories.Requests;

public class GetProductCategoryByIdRequest
{
  public required int Id { get; set; }
}