using System;
using FastEndpoints.Security;
using Microsoft.EntityFrameworkCore;
using reymani_web_api.Api.Endpoints.Auth;
using reymani_web_api.Application.Utils;
using System.IdentityModel.Tokens.Jwt;

namespace reymani_web_api.Application.Services;

public class AuthService : IAuthService
{
  private readonly IClienteRepository _clienteRepository;
  private readonly IConfiguration _configuration;

  public AuthService(IClienteRepository clienteRepository, IConfiguration configuration)
  {
    _clienteRepository = clienteRepository;
    _configuration = configuration;
  }

  public async Task<bool> IsNumeroCarnetInUseAsync(string numeroCarnet)
  {
    var clientes = await _clienteRepository.GetAllAsync();
    return clientes.Any(c => c.NumeroCarnet == numeroCarnet);
  }

  public async Task<bool> IsUsernameInUseAsync(string username)
  {
    var clientes = await _clienteRepository.GetAllAsync();
    return clientes.Any(c => c.Username == username);
  }

  public async Task<string> RegisterAsync(RegisterRequest request)
  {
    var hashedPassword = HashPassword.ComputeHash(request.Password);

    var cliente = new Cliente
    {
      IdCliente = Guid.NewGuid(),
      NumeroCarnet = request.NumeroCarnet,
      Nombre = request.Nombre,
      Apellidos = request.Apellidos,
      Username = request.Username,
      PasswordHash = hashedPassword,
      FechaRegistro = DateTime.UtcNow,
      Activo = true
    };

    await _clienteRepository.AddAsync(cliente);
    var jwtSecret = _configuration["JwtSecret"];
    var jwtToken = JwtBearer.CreateToken(
      o =>
      {
        o.SigningKey = jwtSecret;
        o.ExpireAt = DateTime.UtcNow.AddDays(1);
        o.User.Claims.Add(("IdCliente", cliente.IdCliente.ToString()));
      });

    return jwtToken;
  }

  public async Task<string> LoginAsync(LoginRequest request)
  {
    var cliente = await _clienteRepository.GetClienteByUsernameOrPhoneAsync(request.UsernameOrPhone);
    if (cliente == null || !HashPassword.VerifyHash(request.Password, cliente.PasswordHash))
    {
      throw new UnauthorizedAccessException("Credenciales inválidas");
    }

    var rolesId = await _clienteRepository.GetIdRolesClienteAsync(cliente.IdCliente);
    var jwtSecret = _configuration["JwtSecret"];
    var jwtToken = JwtBearer.CreateToken(
      o =>
      {
        o.SigningKey = jwtSecret;
        o.ExpireAt = DateTime.UtcNow.AddDays(1);
        o.User.Roles.AddRange(rolesId.Where(r => r != null)!);
        o.User.Claims.Add(("IdCliente", cliente.IdCliente.ToString()));
      });

    return jwtToken;
  }

  public async Task<Guid> GetIdClienteFromTokenAsync(string token)
  {
    var jwtSecret = _configuration["JwtSecret"];
    var handler = new JwtSecurityTokenHandler();
    var jsonToken = handler.ReadToken(token) as JwtSecurityToken;

    if (jsonToken == null)
    {
      throw new UnauthorizedAccessException("Token inválido");
    }

    var idClienteClaim = jsonToken.Claims.FirstOrDefault(c => c.Type == "IdCliente");

    if (idClienteClaim == null)
    {
      throw new UnauthorizedAccessException("Token inválido");
    }

    return Guid.Parse(idClienteClaim.Value);
  }
}
