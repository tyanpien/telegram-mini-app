using Microsoft.AspNetCore.Mvc;
using TubeAppBackend.Models;
using TubeAppBackend.Services.Interfaces;

namespace TubeAppBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RemnantController : ControllerBase
    {
        private readonly IRemnantService _remnantService;

        public RemnantController(IRemnantService remnantService)
        {
            _remnantService = remnantService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Remnant>>> GetAll()
        {
            var result = await _remnantService.GetAllRemnantsAsync();
            return Ok(result);
        }

        [HttpGet("nomenclature/{nomenclatureId}")]
        public async Task<ActionResult<List<Remnant>>> GetByNomenclatureId(string nomenclatureId)
        {
            var result = await _remnantService.GetRemnantsByNomenclatureIdAsync(nomenclatureId);
            return Ok(result);
        }

        [HttpGet("stock/{stockId}")]
        public async Task<ActionResult<List<Remnant>>> GetByStockId(string stockId)
        {
            var result = await _remnantService.GetRemnantsByStockIdAsync(stockId);
            return Ok(result);
        }

        [HttpGet("available/{nomenclatureId}")]
        public async Task<ActionResult<List<Remnant>>> GetAvailable(string nomenclatureId)
        {
            var result = await _remnantService.GetAvailableRemnantsAsync(nomenclatureId);
            return Ok(result);
        }

        [HttpGet("total/{nomenclatureId}")]
        public async Task<ActionResult<object>> GetTotalStock(string nomenclatureId)
        {
            var (totalT, totalM) = await _remnantService.GetTotalStockByNomenclatureAsync(nomenclatureId);
            return Ok(new { TotalTons = totalT, TotalMeters = totalM });
        }
    }
}
