using System;
using reymani_web_api.Domain.Entities;

namespace reymani_web_api.Application.Interfaces
{
  public interface INegocioUsuarioRepository
  {
    Task AddAsync(NegocioUsuario negocioUsuario);
    Task DeleteAsync(Guid UsuarioId, Guid negocioId);
    Task<NegocioUsuario?> GetByIdAsync(Guid id);
    Task<IEnumerable<Negocio>> GetNegociosByUsuarioIdAsync(Guid UsuarioId);
    Task<IEnumerable<Usuario>> GetUsuariosByNegocioIdAsync(Guid negocioId);
    Task<NegocioUsuario?> GetByIdUsuarioAndIdNegocio(Guid idUsuario, Guid idNegocio);
  }
}
