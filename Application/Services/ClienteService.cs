using System;
using reymani_web_api.Api.Endpoints.Cliente;
using reymani_web_api.Application.Utils;

namespace reymani_web_api.Application.Services;

public class ClienteService : IClienteService
{
  private readonly IClienteRepository _clienteRepository;
  private readonly IRolRepository _rolRepository;

  public ClienteService(IClienteRepository clienteRepository, IRolRepository rolRepository)
  {
    _clienteRepository = clienteRepository;
    _rolRepository = rolRepository;
  }

  public async Task<IEnumerable<Cliente>> GetAllClientesAsync()
  {
    return await _clienteRepository.GetAllAsync();
  }

  public async Task<Cliente?> GetClienteByIdAsync(Guid id)
  {
    return await _clienteRepository.GetByIdAsync(id);
  }

  public async Task UpdateClienteAsync(Cliente cliente)
  {
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

  public async Task AssignRoleToClienteAsync(Guid clienteId, Guid roleId)
  {
    var cliente = await _clienteRepository.GetByIdAsync(clienteId);
    if (cliente == null)
    {
      throw new Exception("Cliente no encontrado.");
    }

    var rol = await _rolRepository.GetByIdAsync(roleId);
    if (rol == null)
    {
      throw new Exception("Rol no encontrado.");
    }

    cliente.Roles.Add(new ClienteRol { IdCliente = clienteId, IdRol = roleId });
    await _clienteRepository.UpdateAsync(cliente);
  }

  public Task<bool> CheckPasswordAsync(Cliente cliente, string password)
  {
    return Task.FromResult(HashPassword.VerifyHash(password, cliente.PasswordHash));
  }

  public Task ChangePasswordAsync(Cliente cliente, string newPassword)
  {
    cliente.PasswordHash = HashPassword.ComputeHash(newPassword);
    return _clienteRepository.UpdateAsync(cliente);
  }
}
