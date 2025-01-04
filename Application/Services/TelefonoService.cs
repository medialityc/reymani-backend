using System;

namespace reymani_web_api.Application.Services;

public class TelefonoService : ITelefonoService
{
  private readonly ITelefonoRepository _telefonoRepository;

  public TelefonoService(ITelefonoRepository telefonoRepository)
  {
    _telefonoRepository = telefonoRepository;
  }

  public async Task<Telefono> AddAsync(Telefono telefono)
  {
    return await _telefonoRepository.AddAsync(telefono);
  }

  public async Task<Telefono?> GetByIdAsync(Guid id)
  {
    return await _telefonoRepository.GetByIdAsync(id);
  }

  public async Task<IEnumerable<Telefono>> GetAllAsync()
  {
    return await _telefonoRepository.GetAllAsync();
  }

  public async Task UpdateAsync(Telefono telefono)
  {
    await _telefonoRepository.UpdateAsync(telefono);
  }

  public async Task DeleteAsync(Guid id)
  {
    await _telefonoRepository.DeleteAsync(id);
  }
}
