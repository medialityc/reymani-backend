using System;
using reymani_web_api.Application.Interfaces;
using reymani_web_api.Domain.Entities;

namespace reymani_web_api.Application.Services
{
  public class NegocioUsuarioService : INegocioUsuarioService
  {
    private readonly INegocioUsuarioRepository _negocioUsuarioRepository;

    public NegocioUsuarioService(INegocioUsuarioRepository negocioUsuarioRepository)
    {
      _negocioUsuarioRepository = negocioUsuarioRepository;
    }

    public async Task AddAsync(NegocioUsuario negocioUsuario)
    {
      await _negocioUsuarioRepository.AddAsync(negocioUsuario);
    }

    public async Task DeleteAsync(Guid UsuarioId, Guid negocioId)
    {
      await _negocioUsuarioRepository.DeleteAsync(UsuarioId, negocioId);
    }

    public async Task<NegocioUsuario?> GetByIdAsync(Guid id)
    {
      return await _negocioUsuarioRepository.GetByIdAsync(id);
    }

    public async Task<IEnumerable<Negocio>> GetNegociosByUsuarioIdAsync(Guid UsuarioId)
    {
      return await _negocioUsuarioRepository.GetNegociosByUsuarioIdAsync(UsuarioId);
    }

    public async Task<IEnumerable<Usuario>> GetUsuariosByNegocioIdAsync(Guid negocioId)
    {
      return await _negocioUsuarioRepository.GetUsuariosByNegocioIdAsync(negocioId);
    }

    public async Task<NegocioUsuario?> GetByIdUsuarioAndIdNegocio(Guid idUsuario, Guid idNegocio)
    {
      return await _negocioUsuarioRepository.GetByIdUsuarioAndIdNegocio(idUsuario, idNegocio);
    }
  }
}
