using System;

namespace reymani_web_api.Application.Interfaces;

public interface IClienteRepository
{
  Task<IEnumerable<Cliente>> GetAllAsync();
  Task<Cliente?> GetByIdAsync(Guid id);
  Task AddAsync(Cliente cliente);
  Task UpdateAsync(Cliente cliente);
  Task DeleteAsync(Cliente cliente);
  Task<Cliente?> GetClienteByUsernameOrPhoneAsync(string usernameOrPhone);
  Task<string[]> GetCodigosRolesClienteAsync(Guid id);
  Task<string[]> GetCodigosPermisosClienteAsync(Guid id);
}