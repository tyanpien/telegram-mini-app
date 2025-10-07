using System.Text.Json;
using TubeAppBackend.Models;
using TubeAppBackend.Services.Interfaces;

namespace TubeAppBackend.Services
{
    public class PriceService : IPriceService
    {
        private readonly List<Price> _priceData;

        public PriceService(IConfiguration configuration)
        {
            var jsonPath = configuration["DataPaths:Prices"] ?? "Data/prices.json";

            if (!File.Exists(jsonPath))
            {
                _priceData = new List<Price>();
                return;
            }

            var jsonData = File.ReadAllText(jsonPath);

            try
            {
                var data = JsonSerializer.Deserialize<JsonElement>(jsonData);

                if (data.ValueKind == JsonValueKind.Array)
                {
                    _priceData = JsonSerializer.Deserialize<List<Price>>(data.GetRawText()) ?? new List<Price>();
                }
                else if (data.TryGetProperty("ArrayOfPriceEl", out var arrayElement))
                {
                    _priceData = arrayElement.EnumerateArray()
                        .Select(item => JsonSerializer.Deserialize<Price>(item.GetRawText()) ?? new Price())
                        .ToList();
                }
                else
                {
                    _priceData = new List<Price>();
                }
            }
            catch (Exception)
            {
                _priceData = new List<Price>();
            }
        }

        public Task<List<Price>> GetAllPricesAsync()
        {
            return Task.FromResult(_priceData);
        }

        public Task<List<Price>> GetPricesByNomenclatureIdAsync(string nomenclatureId)
        {
            var result = _priceData
                .Where(p => p.ID == nomenclatureId)
                .ToList();
            return Task.FromResult(result);
        }

        public Task<List<Price>> GetPricesByStockIdAsync(string stockId)
        {
            var result = _priceData
                .Where(p => p.IDStock == stockId)
                .ToList();
            return Task.FromResult(result);
        }

        public Task<Price?> GetPriceAsync(string nomenclatureId, string stockId)
        {
            var price = _priceData.FirstOrDefault(p => p.ID == nomenclatureId && p.IDStock == stockId);
            return Task.FromResult(price);
        }

        public Task<decimal> GetBestPriceTAsync(string nomenclatureId)
        {
            var prices = _priceData.Where(p => p.ID == nomenclatureId && p.PriceT > 0).ToList();
            var bestPrice = prices.Any() ? prices.Min(p => p.PriceT) : 0;
            return Task.FromResult(bestPrice);
        }

        public Task<decimal> GetBestPriceMAsync(string nomenclatureId)
        {
            var prices = _priceData.Where(p => p.ID == nomenclatureId && p.PriceM > 0).ToList();
            var bestPrice = prices.Any() ? prices.Min(p => p.PriceM) : 0;
            return Task.FromResult(bestPrice);
        }
    }
}
