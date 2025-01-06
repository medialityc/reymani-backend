using System;

namespace reymani_web_api.Application.Interfaces;

public interface ITelefonoRepository
{
  Task<Telefono> AddAsync(Telefono telefono);
  Task<Telefono?> GetByIdAsync(Guid id);
  Task<IEnumerable<Telefono>> GetAllAsync();
  Task UpdateAsync(Telefono telefono);
  Task DeleteAsync(Guid id);
  Task<Telefono?> GetByNumeroAndEntidadAsync(string numero, Guid idEntidad);
  Task<IEnumerable<Telefono>> GetAllByIdEntidadAsync(Guid idEntidad);
}
