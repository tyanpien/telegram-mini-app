using TubeAppBackend.Models;

namespace TubeAppBackend.Services.Interfaces
{
    public interface IRemnantService
    {
        Task<List<Remnant>> GetAllRemnantsAsync();
        Task<List<Remnant>> GetRemnantsByNomenclatureIdAsync(string nomenclatureId);
        Task<List<Remnant>> GetRemnantsByStockIdAsync(string stockId);
        Task<List<Remnant>> GetAvailableRemnantsAsync(string nomenclatureId);
        Task<(double totalT, double totalM)> GetTotalStockByNomenclatureAsync(string nomenclatureId);
    }
}
