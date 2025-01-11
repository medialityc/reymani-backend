namespace reymani_web_api.Application.Interfaces;

public interface IMetodoPagoRepository
{
  Task<MetodoPago> AddAsync(MetodoPago metodoPago);
  Task<MetodoPago?> GetByIdAsync(Guid id);
  Task<IEnumerable<MetodoPago>> GetAllAsync();
  Task UpdateAsync(MetodoPago metodoPago);
  Task DeleteAsync(Guid id);
  Task<IEnumerable<MetodoPago>> GetAllByIdEntidadAsync(Guid idEntidad);
  Task<bool> ExistsByEntidadAndProveedorAsync(Guid idEntidad, string proveedor);
}
