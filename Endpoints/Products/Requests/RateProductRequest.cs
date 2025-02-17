using System;

using reymani_web_api.Data.Models;

namespace reymani_web_api.Endpoints.Products.Requests;

public class RateProductRequest
{
  public int ProductId { get; set; }
  public RatingPunctuation Rating { get; set; }
}
