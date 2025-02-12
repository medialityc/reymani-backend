using Microsoft.AspNetCore.Http;

namespace reymani_web_api.Endpoints.ProductCategories.Requests
{
  public class CreateProductCategoryRequest
  {
    public required string Name { get; set; }

    public required IFormFile Logo { get; set; }
  }
}
