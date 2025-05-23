namespace reymani_web_api.Endpoints.Business.Requests
{
  public class UpdateMyBusinessRequest
  {
    public required string Name { get; set; }
    public required string? Description { get; set; }
    public required IFormFile? Logo { get; set; }
    public required IFormFile? Banner { get; set; }
    public required string Address { get; set; }
    public required int MunicipalityId { get; set; }
    public required bool IsAvailable { get; set; }
  }
}