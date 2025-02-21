using System;

namespace reymani_web_api.Endpoints.Products.Requests;

public class GetProductByIdRequest
{
  public required int Id { get; set; }
}