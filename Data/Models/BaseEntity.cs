using System;

namespace reymani_web_api.Data.Models;

public abstract class BaseEntity
{
  public DateTimeOffset CreatedAt { get; set; }
  public DateTimeOffset UpdatedAt { get; set; }
}