using System;

namespace reymani_web_api;

public static class DependencyInjectionExtensions
{
  public static IServiceCollection AddDomainServices(this IServiceCollection services)
  {
    services.AddScoped<IRolService, RolService>();
    services.AddScoped<IPermisoService, PermisoService>();
    services.AddScoped<IClienteService, ClienteService>();
    services.AddScoped<IAuthService, AuthService>();
    services.AddScoped<IAuthorizationService, AuthorizationService>();
    services.AddScoped<INegocioService, NegocioService>();
    services.AddScoped<ITelefonoService, TelefonoService>();
    services.AddScoped<IDireccionService, DireccionService>();
    services.AddScoped<ICategoriaNegocioService, CategoriaNegocioService>();

    return services;
  }

  public static IServiceCollection AddRepositories(this IServiceCollection services)
  {
    services.AddScoped<IRolRepository, RolRepository>();
    services.AddScoped<IPermisoRepository, PermisoRepository>();
    services.AddScoped<IClienteRepository, ClienteRepository>();
    services.AddScoped<INegocioRepository, NegocioRepository>();
    services.AddScoped<ITelefonoRepository, TelefonoRepository>();
    services.AddScoped<IDireccionRepository, DireccionRepository>();
    services.AddScoped<ICategoriaNegocioRepository, CategoriaNegocioRepository>();

    return services;
  }

}
