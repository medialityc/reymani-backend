using System;

namespace reymani_web_api;

public static class DependencyInjectionExtensions
{
  public static IServiceCollection AddDomainServices(this IServiceCollection services)
  {
    services.AddScoped<IRolService, RolService>();
    services.AddScoped<IPermisoService, PermisoService>();
    services.AddScoped<IUsuarioService, UsuarioService>();
    services.AddScoped<IAuthService, AuthService>();
    services.AddScoped<IAuthorizationService, AuthorizationService>();
    services.AddScoped<INegocioService, NegocioService>();
    services.AddScoped<ITelefonoService, TelefonoService>();
    services.AddScoped<IDireccionService, DireccionService>();
    services.AddScoped<ICategoriaNegocioService, CategoriaNegocioService>();
    services.AddScoped<ICostoEnvioService, CostoEnvioService>();
    services.AddScoped<IHorarioNegocioService, HorarioNegocioService>();
    services.AddScoped<INegocioUsuarioService, NegocioUsuarioService>();
    services.AddScoped<IMetodoPagoService, MetodoPagoService>();

    return services;
  }

  public static IServiceCollection AddRepositories(this IServiceCollection services)
  {
    services.AddScoped<IRolRepository, RolRepository>();
    services.AddScoped<IPermisoRepository, PermisoRepository>();
    services.AddScoped<IUsuarioRepository, UsuarioRepository>();
    services.AddScoped<INegocioRepository, NegocioRepository>();
    services.AddScoped<ITelefonoRepository, TelefonoRepository>();
    services.AddScoped<IDireccionRepository, DireccionRepository>();
    services.AddScoped<ICategoriaNegocioRepository, CategoriaNegocioRepository>();
    services.AddScoped<ICostoEnvioRepository, CostoEnvioRepository>();
    services.AddScoped<IHorarioNegocioRepository, HorarioNegocioRepository>();
    services.AddScoped<INegocioUsuarioRepository, NegocioUsuarioRepository>();
    services.AddScoped<IMetodoPagoRepository, MetodoPagoRepository>();

    return services;
  }

}
