namespace reymani_web_api.Services.CleanCartServices;

public class CleanCartBackgroundService : BackgroundService
{
  private readonly IServiceProvider _serviceProvider;
  private readonly ILogger<CleanCartBackgroundService> _logger;

  public CleanCartBackgroundService(IServiceProvider serviceProvider, ILogger<CleanCartBackgroundService> logger)
  {
    _serviceProvider = serviceProvider;
    _logger = logger;
  }

  protected override async Task ExecuteAsync(CancellationToken stoppingToken)
  {
    _logger.LogInformation("Servicio de limpieza de carritos iniciado.");

    while (!stoppingToken.IsCancellationRequested)
    {
      try
      {
        // Crear un ámbito para resolver el servicio con ámbito
        using (var scope = _serviceProvider.CreateScope())
        {
          var cleanCartService = scope.ServiceProvider.GetRequiredService<ICleanCartService>();
          await cleanCartService.CleanInactiveCartsAsync(stoppingToken);
        }
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, "Error al limpiar carritos inactivos.");
      }

      // Esperar 12 horas antes de la próxima ejecución
      await Task.Delay(TimeSpan.FromHours(12), stoppingToken);
    }

    _logger.LogInformation("Servicio de limpieza de carritos detenido.");
  }
}
