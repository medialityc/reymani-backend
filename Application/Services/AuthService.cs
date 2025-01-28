using System;
using FastEndpoints.Security;
using Microsoft.EntityFrameworkCore;
using reymani_web_api.Api.Endpoints.Auth;
using reymani_web_api.Application.Utils;
using System.IdentityModel.Tokens.Jwt;

namespace reymani_web_api.Application.Services;

public class AuthService : IAuthService
{
  private readonly IUsuarioRepository _UsuarioRepository;
  private readonly IConfiguration _configuration;

  public AuthService(IUsuarioRepository UsuarioRepository, IConfiguration configuration)
  {
    _UsuarioRepository = UsuarioRepository;
    _configuration = configuration;
  }

  public async Task<bool> IsNumeroCarnetInUseAsync(string numeroCarnet)
  {
    var Usuarios = await _UsuarioRepository.GetAllAsync();
    return Usuarios.Any(c => c.NumeroCarnet == numeroCarnet);
  }

  public async Task<bool> IsUsernameInUseAsync(string username)
  {
    var Usuarios = await _UsuarioRepository.GetAllAsync();
    return Usuarios.Any(c => c.Username == username);
  }

  public async Task<string> RegisterAsync(RegisterRequest request)
  {
    var hashedPassword = HashPassword.ComputeHash(request.Password);

    var Usuario = new Usuario
    {
      IdUsuario = Guid.NewGuid(),
      NumeroCarnet = request.NumeroCarnet,
      Nombre = request.Nombre,
      Apellidos = request.Apellidos,
      Username = request.Username,
      PasswordHash = hashedPassword,
      FechaRegistro = DateTime.UtcNow,
      Activo = true
    };

    await _UsuarioRepository.AddAsync(Usuario);
    var jwtSecret = _configuration["JwtSecret"];
    var jwtToken = JwtBearer.CreateToken(
      o =>
      {
        o.SigningKey = jwtSecret;
        o.ExpireAt = DateTime.UtcNow.AddDays(1);
        o.User.Claims.Add(("IdUsuario", Usuario.IdUsuario.ToString()));
      });

    return jwtToken;
  }

  public async Task<string> LoginAsync(LoginRequest request)
  {
    var Usuario = await _UsuarioRepository.GetUsuarioByUsernameOrPhoneAsync(request.UsernameOrPhone);
    if (Usuario == null || !HashPassword.VerifyHash(request.Password, Usuario.PasswordHash) || !Usuario.Activo)
    {
      throw new UnauthorizedAccessException("Credenciales inválidas");
    }

    var rolesId = await _UsuarioRepository.GetIdRolesUsuarioAsync(Usuario.IdUsuario);
    var jwtSecret = _configuration["JwtSecret"];
    var jwtToken = JwtBearer.CreateToken(
      o =>
      {
        o.SigningKey = jwtSecret;
        o.ExpireAt = DateTime.UtcNow.AddDays(1);
        o.User.Roles.AddRange(rolesId.Where(r => r != null)!);
        o.User.Claims.Add(("IdUsuario", Usuario.IdUsuario.ToString()));
      });

    return jwtToken;
  }

  public Guid GetIdUsuarioFromToken(string token)
  {
    var jwtSecret = _configuration["JwtSecret"];
    var handler = new JwtSecurityTokenHandler();
    var jsonToken = handler.ReadToken(token) as JwtSecurityToken;

    if (jsonToken == null)
    {
      throw new UnauthorizedAccessException("Token inválido");
    }

    var idUsuarioClaim = jsonToken.Claims.FirstOrDefault(c => c.Type == "IdUsuario");

    if (idUsuarioClaim == null)
    {
      throw new UnauthorizedAccessException("Token inválido");
    }

    return Guid.Parse(idUsuarioClaim.Value);
  }
}
