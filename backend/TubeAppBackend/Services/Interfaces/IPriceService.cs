using TubeAppBackend.Models;

namespace TubeAppBackend.Services.Interfaces
{
    public interface IPriceService
    {
        Task<List<Price>> GetAllPricesAsync();
        Task<List<Price>> GetPricesByNomenclatureIdAsync(string nomenclatureId);
        Task<List<Price>> GetPricesByStockIdAsync(string stockId);
        Task<Price?> GetPriceAsync(string nomenclatureId, string stockId);
        Task<decimal> GetBestPriceTAsync(string nomenclatureId);
        Task<decimal> GetBestPriceMAsync(string nomenclatureId);
    }
}
