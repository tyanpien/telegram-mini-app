using TubeAppBackend.Models;

namespace TubeAppBackend.Services.Interfaces
{
    public interface INomenclatureService
    {
        Task<List<Nomenclature>> GetAllNomenclatureAsync();
        Task<Nomenclature?> GetNomenclatureByIdAsync(string id);
        Task<List<Nomenclature>> GetNomenclatureByTypeAsync(string typeId);
        Task<List<Nomenclature>> SearchNomenclatureAsync(string searchTerm);
        Task<List<Nomenclature>> GetNomenclatureByDiameterRangeAsync(double minDiameter, double maxDiameter);
    }
}
