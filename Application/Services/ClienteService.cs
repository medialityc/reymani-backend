using System;
using reymani_web_api.Api.Endpoints.Cliente;
using reymani_web_api.Application.Utils;

namespace reymani_web_api.Application.Services;

public class ClienteService : IClienteService
{
  private readonly IClienteRepository _clienteRepository;

  public ClienteService(IClienteRepository clienteRepository)
  {
    _clienteRepository = clienteRepository;
  }

  public async Task CreateClienteAsync(CreateClienteRequest request)
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
  }

  public async Task<IEnumerable<Cliente>> GetAllClientesAsync()
  {
    return await _clienteRepository.GetAllAsync();
  }

  public async Task<Cliente?> GetClienteByIdAsync(Guid id)
  {
    return await _clienteRepository.GetByIdAsync(id);
  }

  public async Task UpdateClienteAsync(UpdateClienteRequest request)
  {
    var cliente = await _clienteRepository.GetByIdAsync(request.IdCliente);

    if (cliente == null)
    {
      throw new Exception("Cliente no encontrado.");
    }

    cliente.NumeroCarnet = request.NumeroCarnet;
    cliente.Nombre = request.Nombre;
    cliente.Apellidos = request.Apellidos;
    cliente.Username = request.Username;
    cliente.PasswordHash = request.Password;

    await _clienteRepository.UpdateAsync(cliente);
  }

  public async Task DeleteClienteAsync(Guid id)
  {
    var cliente = await _clienteRepository.GetByIdAsync(id);

    if (cliente == null)
    {
      throw new Exception("Cliente no encontrado.");
    }

    await _clienteRepository.DeleteAsync(cliente);

  }

}
