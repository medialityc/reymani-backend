namespace reymani_web_api.Application.Services
{
  public class HorarioNegocioService : IHorarioNegocioService
  {
    private readonly IHorarioNegocioRepository _horarioNegocioRepository;

    public HorarioNegocioService(IHorarioNegocioRepository horarioNegocioRepository)
    {
      _horarioNegocioRepository = horarioNegocioRepository;
    }

    public async Task<IEnumerable<HorarioNegocio>> GetAllAsync()
    {
      return await _horarioNegocioRepository.GetAllAsync();
    }

    public async Task<HorarioNegocio?> GetByIdAsync(Guid id)
    {
      return await _horarioNegocioRepository.GetByIdAsync(id);
    }

    public async Task AddAsync(HorarioNegocio horarioNegocio)
    {
      await _horarioNegocioRepository.AddAsync(horarioNegocio);
    }

    public async Task UpdateAsync(HorarioNegocio horarioNegocio)
    {
      await _horarioNegocioRepository.UpdateAsync(horarioNegocio);
    }

    public async Task DeleteAsync(Guid id)
    {
      var horarioNegocio = await GetByIdAsync(id);
      if (horarioNegocio == null)
      {
        throw new Exception("Horario de negocio no encontrado.");
      }
      await _horarioNegocioRepository.DeleteAsync(horarioNegocio);
    }

    public async Task<bool> HorarioExistsForDiaAsync(Guid idNegocio, int dia)
    {
      return await _horarioNegocioRepository.HorarioExistsForDiaAsync(idNegocio, dia);
    }
  }
}
