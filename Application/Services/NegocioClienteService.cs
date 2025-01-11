using System;
using reymani_web_api.Application.Interfaces;
using reymani_web_api.Domain.Entities;

namespace reymani_web_api.Application.Services
{
  public class NegocioClienteService : INegocioClienteService
  {
    private readonly INegocioClienteRepository _negocioClienteRepository;

    public NegocioClienteService(INegocioClienteRepository negocioClienteRepository)
    {
      _negocioClienteRepository = negocioClienteRepository;
    }

    public async Task AddAsync(NegocioCliente negocioCliente)
    {
      await _negocioClienteRepository.AddAsync(negocioCliente);
    }

    public async Task DeleteAsync(Guid clienteId, Guid negocioId)
    {
      await _negocioClienteRepository.DeleteAsync(clienteId, negocioId);
    }

    public async Task<NegocioCliente?> GetByIdAsync(Guid id)
    {
      return await _negocioClienteRepository.GetByIdAsync(id);
    }

    public async Task<IEnumerable<Negocio>> GetNegociosByClienteIdAsync(Guid clienteId)
    {
      return await _negocioClienteRepository.GetNegociosByClienteIdAsync(clienteId);
    }

    public async Task<IEnumerable<Cliente>> GetClientesByNegocioIdAsync(Guid negocioId)
    {
      return await _negocioClienteRepository.GetClientesByNegocioIdAsync(negocioId);
    }

    public async Task<NegocioCliente?> GetByIdClienteAndIdNegocio(Guid idCliente, Guid idNegocio)
    {
      return await _negocioClienteRepository.GetByIdClienteAndIdNegocio(idCliente, idNegocio);
    }
  }
}
