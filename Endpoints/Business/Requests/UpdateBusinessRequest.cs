namespace reymani_web_api.Endpoints.Business.Requests
{
  public class UpdateBusinessRequest
  {
    public required int Id { get; set; }
    public required string Name { get; set; }
    public required string? Description { get; set; }
    public required IFormFile? Logo { get; set; }
    public required IFormFile? Banner { get; set; }
    public required int UserId { get; set; } // Administrador del negocio
    public required string Address { get; set; }
    public required int MunicipalityId { get; set; }
    public required bool IsAvailable { get; set; }
    public required bool IsActive { get; set; }
  }
}
