using System;

using ReymaniWebApi.Data.Models;

namespace reymani_web_api.Data.Models;

public class ProductRating : BaseEntity
{
  public int Id { get; set; }
  public required int ProductId { get; set; }
  public Product? Product { get; set; }
  public required int UserId { get; set; }
  public User? User { get; set; }
  public required RatingPunctuation Rating { get; set; }
}
