using TubeAppBackend.Models;

namespace TubeAppBackend.Services.Interfaces
{
    public interface IStockService
    {
        Task<List<Stock>> GetAllStockAsync();
        Task<Stock?> GetStockByIdAsync(string stockId);
        Task<List<Stock>> GetStockByCityAsync(string city);
        Task<List<Stock>> GetStockByNameAsync(string stockName);
    }
}
