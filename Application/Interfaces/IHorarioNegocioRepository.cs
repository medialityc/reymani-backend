namespace reymani_web_api.Application.Interfaces
{
  public interface IHorarioNegocioRepository
  {
    Task<IEnumerable<HorarioNegocio>> GetAllAsync();
    Task<HorarioNegocio?> GetByIdAsync(Guid id);
    Task AddAsync(HorarioNegocio horarioNegocio);
    Task UpdateAsync(HorarioNegocio horarioNegocio);
    Task DeleteAsync(HorarioNegocio horarioNegocio);
  }
}
