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

    return services;
  }

  public static IServiceCollection AddRepositories(this IServiceCollection services)
  {
    services.AddScoped<IRolRepository, RolRepository>();
    services.AddScoped<IPermisoRepository, PermisoRepository>();
    services.AddScoped<IClienteRepository, ClienteRepository>();

    return services;
  }

}
