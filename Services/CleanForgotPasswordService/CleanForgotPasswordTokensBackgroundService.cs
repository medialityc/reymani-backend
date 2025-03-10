namespace reymani_web_api.Services.CleanForgotPasswordService;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using System;
using System.Threading;
using System.Threading.Tasks;

public class CleanForgotPasswordTokensBackgroundService : BackgroundService
{
  private readonly IServiceProvider _serviceProvider;
  private readonly ILogger<CleanForgotPasswordTokensBackgroundService> _logger;

  public CleanForgotPasswordTokensBackgroundService(IServiceProvider serviceProvider, ILogger<CleanForgotPasswordTokensBackgroundService> logger)
  {
    _serviceProvider = serviceProvider;
    _logger = logger;
  }

  protected override async Task ExecuteAsync(CancellationToken stoppingToken)
  {
    _logger.LogInformation("Servicio de limpieza de tokens de recuperación de contraseña iniciado.");

    while (!stoppingToken.IsCancellationRequested)
    {
      try
      {
        // Crear un ámbito para resolver el servicio con ámbito
        using (var scope = _serviceProvider.CreateScope())
        {
          var cleanTokensService = scope.ServiceProvider.GetRequiredService<ICleanForgotPasswordTokensService>();
          await cleanTokensService.CleanExpiredTokensAsync(stoppingToken);
        }
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, "Error al limpiar tokens de recuperación de contraseña.");
      }

      // Esperar 12 horas antes de la próxima ejecución
      await Task.Delay(TimeSpan.FromHours(12), stoppingToken);
    }

    _logger.LogInformation("Servicio de limpieza de tokens de recuperación de contraseña detenido.");
  }
}
