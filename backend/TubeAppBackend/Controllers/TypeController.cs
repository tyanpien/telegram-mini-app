using Microsoft.AspNetCore.Mvc;
using TubeAppBackend.Models;
using TubeAppBackend.Services.Interfaces;

namespace TubeAppBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TypeController : ControllerBase
    {
        private readonly ITypeService _typeService;

        public TypeController(ITypeService typeService)
        {
            _typeService = typeService;
        }

        [HttpGet]
        public async Task<ActionResult<List<ProductType>>> GetAll()
        {
            var result = await _typeService.GetAllTypesAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductType>> GetById(string id)
        {
            var result = await _typeService.GetTypeByIdAsync(id);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpGet("parent/{parentId}")]
        public async Task<ActionResult<List<ProductType>>> GetByParent(string parentId)
        {
            var result = await _typeService.GetTypesByParentAsync(parentId);
            return Ok(result);
        }
    }
}
