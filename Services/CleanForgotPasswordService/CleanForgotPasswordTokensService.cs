namespace reymani_web_api.Services.CleanForgotPasswordService;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using reymani_web_api.Data;
using reymani_web_api.Data.Models;

using System;
using System.Threading;
using System.Threading.Tasks;

public class CleanForgotPasswordTokensService : ICleanForgotPasswordTokensService
{
  private readonly AppDbContext _dbContext;
  private readonly ILogger<CleanForgotPasswordTokensService> _logger;

  public CleanForgotPasswordTokensService(AppDbContext dbContext, ILogger<CleanForgotPasswordTokensService> logger)
  {
    _dbContext = dbContext;
    _logger = logger;
  }

  public async Task CleanExpiredTokensAsync(CancellationToken cancellationToken = default)
  {
    _logger.LogInformation("Iniciando limpieza de tokens de recuperación de contraseña expirados...");

    // Obtener tokens expirados
    var expiredTokens = await GetExpiredTokensAsync(cancellationToken);

    foreach (var token in expiredTokens)
    {
      await RemoveTokenAsync(token, cancellationToken);
      _logger.LogInformation($"Token {token.Id} eliminado.");
    }

    _logger.LogInformation("Limpieza de tokens de recuperación de contraseña completada.");
  }

  private async Task<IEnumerable<ForgotPasswordNumber>> GetExpiredTokensAsync(CancellationToken cancellationToken)
  {
    var cutoffTime = DateTime.UtcNow.AddHours(-24);
    var expiredTokens = await _dbContext.ForgotPasswordNumbers
        .Where(token => token.UpdatedAt <= cutoffTime)
        .ToListAsync(cancellationToken);

    return expiredTokens;
  }

  private async Task RemoveTokenAsync(ForgotPasswordNumber token, CancellationToken cancellationToken)
  {
    _dbContext.ForgotPasswordNumbers.Remove(token);
    await _dbContext.SaveChangesAsync(cancellationToken);
  }
}
