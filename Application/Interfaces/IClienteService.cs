using System;
using reymani_web_api.Api.Endpoints.Cliente;

namespace reymani_web_api.Application.Interfaces;

public interface IClienteService
{
  Task<IEnumerable<Cliente>> GetAllClientesAsync();
  Task<Cliente?> GetClienteByIdAsync(Guid id);
  Task CreateClienteAsync(CreateClienteRequest request);
  Task UpdateClienteAsync(UpdateClienteRequest request);
  Task DeleteClienteAsync(Guid id);
  Task AssignRoleToClienteAsync(Guid clienteId, Guid roleId);
}
