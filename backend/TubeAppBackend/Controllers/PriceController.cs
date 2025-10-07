using Microsoft.AspNetCore.Mvc;
using TubeAppBackend.Models;
using TubeAppBackend.Services.Interfaces;

namespace TubeAppBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PriceController : ControllerBase
    {
        private readonly IPriceService _priceService;
        private readonly IPriceCalculationService _priceCalculationService;

        public PriceController(IPriceService priceService, IPriceCalculationService priceCalculationService)
        {
            _priceService = priceService;
            _priceCalculationService = priceCalculationService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Price>>> GetAll()
        {
            var result = await _priceService.GetAllPricesAsync();
            return Ok(result);
        }

        [HttpGet("nomenclature/{nomenclatureId}")]
        public async Task<ActionResult<List<Price>>> GetByNomenclatureId(string nomenclatureId)
        {
            var result = await _priceService.GetPricesByNomenclatureIdAsync(nomenclatureId);
            return Ok(result);
        }

        [HttpGet("stock/{stockId}")]
        public async Task<ActionResult<List<Price>>> GetByStockId(string stockId)
        {
            var result = await _priceService.GetPricesByStockIdAsync(stockId);
            return Ok(result);
        }

        [HttpGet("best/ton/{nomenclatureId}")]
        public async Task<ActionResult<decimal>> GetBestPricePerTon(string nomenclatureId)
        {
            var result = await _priceService.GetBestPriceTAsync(nomenclatureId);
            return Ok(result);
        }

        [HttpGet("best/meter/{nomenclatureId}")]
        public async Task<ActionResult<decimal>> GetBestPricePerMeter(string nomenclatureId)
        {
            var result = await _priceService.GetBestPriceMAsync(nomenclatureId);
            return Ok(result);
        }

        [HttpGet("specific")]
        public async Task<ActionResult<Price>> GetSpecificPrice([FromQuery] string nomenclatureId, [FromQuery] string stockId)
        {
            var result = await _priceService.GetPriceAsync(nomenclatureId, stockId);
            if (result == null) return NotFound();
            return Ok(result);
        }

        // метод для получения DTO с лучшей ценой
        [HttpGet("best")]
        public async Task<ActionResult<PriceDTO>> GetBestPrice(
            [FromQuery] string nomenclatureId,
            [FromQuery] decimal quantity = 1,
            [FromQuery] string unit = "ton")
        {
            try
            {
                var result = await _priceCalculationService.CalculateBestPriceAsync(nomenclatureId, quantity, unit);
                if (result == null)
                    return NotFound("Цены не найдены для указанной номенклатуры");

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ошибка при получении лучшей цены: {ex.Message}");
            }
        }

        // метод для получения всех цен с расчетами
        [HttpGet("all")]
        public async Task<ActionResult<List<PriceDTO>>> GetAllPrices(
            [FromQuery] string nomenclatureId,
            [FromQuery] decimal quantity = 1,
            [FromQuery] string unit = "ton")
        {
            try
            {
                var result = await _priceCalculationService.GetAllPricesForNomenclatureAsync(nomenclatureId, quantity, unit);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ошибка при получении цен: {ex.Message}");
            }
        }

        // метод для расчета полной стоимости
        [HttpGet("calculate")]
        public async Task<ActionResult<decimal>> CalculateTotalCost(
            [FromQuery] string nomenclatureId,
            [FromQuery] string stockId,
            [FromQuery] decimal quantity,
            [FromQuery] string unit = "ton")
        {
            try
            {
                var result = await _priceCalculationService.CalculateTotalCostAsync(nomenclatureId, stockId, quantity, unit);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ошибка при расчете стоимости: {ex.Message}");
            }
        }
    }
}
