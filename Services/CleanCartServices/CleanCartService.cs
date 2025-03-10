
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using reymani_web_api.Data;
using reymani_web_api.Services.CleanCartServices;

using ReymaniWebApi.Data.Models;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

public class CleanCartService : ICleanCartService
{
  private readonly AppDbContext _dbContext;
  private readonly ILogger<CleanCartService> _logger;

  public CleanCartService(AppDbContext dbContext, ILogger<CleanCartService> logger)
  {
    _dbContext = dbContext;
    _logger = logger;
  }

  public async Task CleanInactiveCartsAsync(CancellationToken cancellationToken = default)
  {
    _logger.LogInformation("Iniciando limpieza de carritos inactivos...");

    // Obtener carritos inactivos
    var carts = await GetCartsAsync(cancellationToken);

    foreach (var cart in carts)
    {
      await CleanCartAsync(cart, cancellationToken);
      _logger.LogInformation($"Carrito {cart.Id} limpiado.");
    }

    _logger.LogInformation("Limpieza de carritos inactivos completada.");
  }

  private async Task<IEnumerable<ShoppingCart>> GetCartsAsync(CancellationToken cancellationToken)
  {
    var cutoffTime = DateTime.UtcNow.AddHours(-24);
    var inactiveCarts = await _dbContext.ShoppingCarts
        .Where(cart => cart.UpdatedAt <= cutoffTime)
        .ToListAsync(cancellationToken);

    return inactiveCarts;
  }

  private async Task CleanCartAsync(ShoppingCart cart, CancellationToken cancellationToken)
  {
    cart.Items?.Clear();
    await _dbContext.SaveChangesAsync(cancellationToken);
  }
}