using System;
using reymani_web_api.Api.Endpoints.Cliente;

namespace reymani_web_api.Application.Interfaces;

public interface IClienteService
{
  Task<IEnumerable<Cliente>> GetAllClientesAsync();
  Task<Cliente?> GetClienteByIdAsync(Guid id);
  Task UpdateClienteAsync(Cliente cliente);
  Task DeleteClienteAsync(Guid id);
  Task AssignRoleToClienteAsync(Guid clienteId, Guid roleId);
}
