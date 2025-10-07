using Microsoft.AspNetCore.Mvc;
using TubeAppBackend.Models;
using TubeAppBackend.Services.Interfaces;

namespace TubeAppBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StockController : ControllerBase
    {
        private readonly IStockService _stockService;

        public StockController(IStockService stockService)
        {
            _stockService = stockService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Stock>>> GetAll()
        {
            var result = await _stockService.GetAllStockAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Stock>> GetById(string id)
        {
            var result = await _stockService.GetStockByIdAsync(id);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpGet("city/{city}")]
        public async Task<ActionResult<List<Stock>>> GetByCity(string city)
        {
            var result = await _stockService.GetStockByCityAsync(city);
            return Ok(result);
        }

        [HttpGet("name/{name}")]
        public async Task<ActionResult<List<Stock>>> GetByName(string name)
        {
            var result = await _stockService.GetStockByNameAsync(name);
            return Ok(result);
        }
    }
}
