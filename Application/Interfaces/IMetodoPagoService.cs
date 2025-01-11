
namespace reymani_web_api.Application.Interfaces;

public interface IMetodoPagoService
{
  Task<MetodoPago> AddAsync(MetodoPago metodoPago);
  Task<MetodoPago?> GetByIdAsync(Guid id);
  Task<IEnumerable<MetodoPago>> GetAllAsync();
  Task UpdateAsync(MetodoPago metodoPago);
  Task DeleteAsync(Guid id);
  Task<IEnumerable<MetodoPago>> GetAllByIdEntidadAsync(Guid idEntidad);
}
