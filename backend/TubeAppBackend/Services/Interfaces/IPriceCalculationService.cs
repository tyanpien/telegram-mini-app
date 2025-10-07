using TubeAppBackend.Models;

namespace TubeAppBackend.Services.Interfaces
{
    public interface IPriceCalculationService
    {
        Task<PriceDTO?> CalculateBestPriceAsync(string nomenclatureId, decimal quantity, string unit);
        Task<List<PriceDTO>> GetAllPricesForNomenclatureAsync(string nomenclatureId, decimal quantity, string unit);
        Task<decimal> CalculateTotalCostAsync(string nomenclatureId, string stockId, decimal quantity, string unit);
    }
}
