using System;
using reymani_web_api.Domain.Entities;

namespace reymani_web_api.Application.Interfaces
{
  public interface INegocioClienteService
  {
    Task AddAsync(NegocioCliente negocioCliente);
    Task DeleteAsync(Guid id);
    Task<IEnumerable<Negocio>> GetNegociosByClienteIdAsync(Guid clienteId);
    Task<IEnumerable<Cliente>> GetClientesByNegocioIdAsync(Guid negocioId);
  }
}
