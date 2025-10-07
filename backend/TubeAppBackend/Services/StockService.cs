using System.Text.Json;
using TubeAppBackend.Models;
using TubeAppBackend.Services.Interfaces;

namespace TubeAppBackend.Services
{
    public class StockService : IStockService
    {
        private readonly List<Stock> _stocks;

        public StockService(IConfiguration configuration)
        {
            var jsonPath = configuration["DataPaths:Stock"] ?? "Data/stock.json";

            if (!File.Exists(jsonPath))
            {
                _stocks = new List<Stock>();
                return;
            }

            var jsonData = File.ReadAllText(jsonPath);

            try
            {
                var data = JsonSerializer.Deserialize<JsonElement>(jsonData);

                if (data.ValueKind == JsonValueKind.Array)
                {
                    _stocks = JsonSerializer.Deserialize<List<Stock>>(data.GetRawText()) ?? new List<Stock>();
                }
                else if (data.TryGetProperty("ArrayOfStockEl", out var arrayElement))
                {
                    _stocks = arrayElement.EnumerateArray()
                        .Select(item => JsonSerializer.Deserialize<Stock>(item.GetRawText()) ?? new Stock())
                        .ToList();
                }
                else
                {
                    _stocks = new List<Stock>();
                }
            }
            catch (Exception)
            {
                _stocks = new List<Stock>();
            }
        }

        public Task<List<Stock>> GetAllStockAsync()
        {
            return Task.FromResult(_stocks);
        }

        public Task<Stock?> GetStockByIdAsync(string stockId)
        {
            var stock = _stocks.FirstOrDefault(s => s.IDStock == stockId);
            return Task.FromResult(stock);
        }

        public Task<List<Stock>> GetStockByCityAsync(string city)
        {
            var result = _stocks
                .Where(s => s.StockCity?.Contains(city, StringComparison.OrdinalIgnoreCase) == true)
                .ToList();
            return Task.FromResult(result);
        }

        public Task<List<Stock>> GetStockByNameAsync(string stockName)
        {
            var result = _stocks
                .Where(s => s.StockName?.Contains(stockName, StringComparison.OrdinalIgnoreCase) == true)
                .ToList();
            return Task.FromResult(result);
        }
    }
}
