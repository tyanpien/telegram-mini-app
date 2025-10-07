using Microsoft.AspNetCore.Mvc;
using TubeAppBackend.Models;
using TubeAppBackend.Services.Interfaces;

namespace TubeAppBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NomenclatureController : ControllerBase
    {
        private readonly INomenclatureService _nomenclatureService;

        public NomenclatureController(INomenclatureService nomenclatureService)
        {
            _nomenclatureService = nomenclatureService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Nomenclature>>> GetAll()
        {
            var result = await _nomenclatureService.GetAllNomenclatureAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Nomenclature>> GetById(string id)
        {
            var result = await _nomenclatureService.GetNomenclatureByIdAsync(id);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpGet("type/{type}")]
        public async Task<ActionResult<List<Nomenclature>>> GetByType(string type)
        {
            var result = await _nomenclatureService.GetNomenclatureByTypeAsync(type);
            return Ok(result);
        }

        [HttpGet("search")]
        public async Task<ActionResult<List<Nomenclature>>> Search([FromQuery] string term)
        {
            var result = await _nomenclatureService.SearchNomenclatureAsync(term);
            return Ok(result);
        }
    }
}
