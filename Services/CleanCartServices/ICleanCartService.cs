namespace reymani_web_api.Services.CleanCartServices;

public interface ICleanCartService
{
  Task CleanInactiveCartsAsync(CancellationToken cancellationToken = default);
}
