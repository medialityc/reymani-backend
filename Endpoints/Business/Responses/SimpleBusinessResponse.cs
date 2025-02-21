using System;

namespace reymani_web_api.Endpoints.Business.Responses;

public class SimpleBusinessResponse
{
  public required int Id { get; set; }
  public required string Name { get; set; }
  public required string? Description { get; set; }
  public required string Address { get; set; }
  public required int MunicipalityId { get; set; }
  public required bool IsAvailable { get; set; }
  public required string Logo { get; set; }
  public required string Banner { get; set; }
}