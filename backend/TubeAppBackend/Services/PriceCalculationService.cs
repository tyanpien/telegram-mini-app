using TubeAppBackend.Models;
using TubeAppBackend.Services.Interfaces;

namespace TubeAppBackend.Services
{
    public class PriceCalculationService : IPriceCalculationService
    {
        private readonly IPriceService _priceService;
        private readonly IStockService _stockService;

        public PriceCalculationService(IPriceService priceService, IStockService stockService)
        {
            _priceService = priceService;
            _stockService = stockService;
        }

        public async Task<PriceDTO?> CalculateBestPriceAsync(string nomenclatureId, decimal quantity, string unit)
        {
            var allPrices = await _priceService.GetPricesByNomenclatureIdAsync(nomenclatureId);
            if (allPrices == null || !allPrices.Any())
                return null;

            var bestPrice = FindBestPrice(allPrices, quantity, unit);
            if (bestPrice == null)
                return null;

            var stock = await _stockService.GetStockByIdAsync(bestPrice.IDStock);

            var priceValue = GetPriceByUnit(bestPrice, quantity, unit);

            return new PriceDTO
            {
                NomenclatureId = nomenclatureId,
                StockId = bestPrice.IDStock,
                StockName = stock?.StockName ?? "Неизвестный склад",
                StockCity = stock?.StockCity ?? "Неизвестный город",
                PriceValue = priceValue,
                Currency = "RUB",
                Unit = unit,
                NDS = bestPrice.NDS,
                PriceWithNDS = priceValue * (1 + bestPrice.NDS / 100),
                PriceType = GetPriceType(bestPrice, quantity, unit),
                VolumeLimit = GetVolumeLimit(bestPrice, unit)
            };
        }

        public async Task<List<PriceDTO>> GetAllPricesForNomenclatureAsync(string nomenclatureId, decimal quantity, string unit)
        {
            var prices = await _priceService.GetPricesByNomenclatureIdAsync(nomenclatureId);
            var result = new List<PriceDTO>();

            if (prices == null || !prices.Any())
                return result;

            foreach (var price in prices)
            {
                var stock = await _stockService.GetStockByIdAsync(price.IDStock);
                var priceValue = GetPriceByUnit(price, quantity, unit);

                result.Add(new PriceDTO
                {
                    NomenclatureId = nomenclatureId,
                    StockId = price.IDStock,
                    StockName = stock?.StockName ?? "Неизвестный склад",
                    StockCity = stock?.StockCity ?? "Неизвестный город",
                    PriceValue = priceValue,
                    Currency = "RUB",
                    Unit = unit,
                    NDS = price.NDS,
                    PriceWithNDS = priceValue * (1 + price.NDS / 100),
                    PriceType = GetPriceType(price, quantity, unit),
                    VolumeLimit = GetVolumeLimit(price, unit)
                });
            }

            return result.OrderBy(p => p.PriceValue).ToList();
        }

        public async Task<decimal> CalculateTotalCostAsync(string nomenclatureId, string stockId, decimal quantity, string unit)
        {
            var price = await _priceService.GetPriceAsync(nomenclatureId, stockId);
            if (price == null) return 0;

            var pricePerUnit = GetPriceByUnit(price, quantity, unit);
            return pricePerUnit * quantity * (1 + price.NDS / 100);
        }

        private Price? FindBestPrice(List<Price> prices, decimal quantity, string unit)
        {
            var validPrices = prices
                .Where(p => GetPriceByUnit(p, quantity, unit) > 0)
                .ToList();

            if (!validPrices.Any())
                return null;

            return validPrices
                .OrderBy(p => GetPriceByUnit(p, quantity, unit))
                .FirstOrDefault();
        }

        private decimal GetPriceByUnit(Price price, decimal quantity, string unit)
        {
            if (unit.ToLower() == "meter")
            {
                // расчет цены за метр с учетом порогов
                if (price.PriceLimitM2 > 0 && quantity >= price.PriceLimitM2 && price.PriceM2 > 0)
                    return price.PriceM2;
                else if (price.PriceLimitM1 > 0 && quantity >= price.PriceLimitM1 && price.PriceM1 > 0)
                    return price.PriceM1;
                else
                    return price.PriceM > 0 ? price.PriceM : 0;
            }
            else
            {
                // расчет цены за тонну с учетом порогов
                if (price.PriceLimitT2 > 0 && quantity >= price.PriceLimitT2 && price.PriceT2 > 0)
                    return price.PriceT2;
                else if (price.PriceLimitT1 > 0 && quantity >= price.PriceLimitT1 && price.PriceT1 > 0)
                    return price.PriceT1;
                else
                    return price.PriceT > 0 ? price.PriceT : 0;
            }
        }

        private string GetPriceType(Price price, decimal quantity, string unit)
        {
            if (unit.ToLower() == "meter")
            {
                if (price.PriceLimitM2 > 0 && quantity >= price.PriceLimitM2 && price.PriceM2 > 0)
                    return "tier2";
                else if (price.PriceLimitM1 > 0 && quantity >= price.PriceLimitM1 && price.PriceM1 > 0)
                    return "tier1";
                else
                    return "base";
            }
            else
            {
                if (price.PriceLimitT2 > 0 && quantity >= price.PriceLimitT2 && price.PriceT2 > 0)
                    return "tier2";
                else if (price.PriceLimitT1 > 0 && quantity >= price.PriceLimitT1 && price.PriceT1 > 0)
                    return "tier1";
                else
                    return "base";
            }
        }

        private decimal GetVolumeLimit(Price price, string unit)
        {
            if (unit.ToLower() == "meter")
            {
                if (price.PriceLimitM2 > 0) return price.PriceLimitM2;
                if (price.PriceLimitM1 > 0) return price.PriceLimitM1;
                return 0;
            }
            else
            {
                if (price.PriceLimitT2 > 0) return price.PriceLimitT2;
                if (price.PriceLimitT1 > 0) return price.PriceLimitT1;
                return 0;
            }
        }
    }
}
