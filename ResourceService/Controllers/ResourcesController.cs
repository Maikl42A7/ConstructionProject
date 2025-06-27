using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ResourceService.Models;
using ResourceService.Services.Interfaces;
using ResourceService.DTO;

namespace ResourceService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = Roles.Administrator)]
    public class ResourcesController : ControllerBase
    {
        private readonly IResourceRepository _repository;

        public ResourcesController(IResourceRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("techniques")]
        public async Task<ActionResult<List<Technique>>> GetAllTechniques()
        {
            return Ok(await _repository.GetAllTechniquesAsync());
        }

        [HttpGet("materials")]
        public async Task<ActionResult<List<Material>>> GetAllMaterials()
        {
            return Ok(await _repository.GetAllMaterialsAsync());
        }

        [HttpPost("techniques")]
        public async Task<IActionResult> AddTechnique([FromBody] CreateUpdateTechniqueDto dto)
        {
            var technique = new Technique
            {
                Name = dto.Name,
                IsAvailable = dto.IsAvailable,
                Location = dto.Location
            };
            await _repository.AddTechniqueAsync(technique);
            return Ok(technique);
        }

        [HttpPost("materials")]
        public async Task<IActionResult> AddMaterial([FromBody] CreateUpdateMaterialDto dto)
        {
            var material = new Material
            {
                Name = dto.Name,
                Unit = dto.Unit,
                Quantity = dto.Quantity
            };
            await _repository.AddMaterialAsync(material);
            return Ok(material);
        }

        [HttpDelete("techniques/{id}")]
        public async Task<IActionResult> DeleteTechnique(Guid id)
        {
            var result = await _repository.DeleteTechniqueAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }

        [HttpDelete("materials/{id}")]
        public async Task<IActionResult> DeleteMaterial(Guid id)
        {
            var result = await _repository.DeleteMaterialAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }

        [HttpPut("materials/{id}")]
        public async Task<IActionResult> UpdateMaterial(Guid id, [FromBody] CreateUpdateMaterialDto dto)
        {
            var material = await _repository.GetMaterialByIdAsync(id);
            if (material == null)
            {
                return NotFound();
            }

            material.Name = dto.Name;
            material.Unit = dto.Unit;
            material.Quantity = dto.Quantity;

            await _repository.UpdateMaterialAsync(material);
            return NoContent();
        }

        [HttpPut("techniques/{id}")]
        public async Task<IActionResult> UpdateTechnique(Guid id, [FromBody] CreateUpdateTechniqueDto dto)
        {
            var technique = await _repository.GetTechniqueByIdAsync(id);
            if (technique == null)
            {
                return NotFound();
            }

            technique.Name = dto.Name;
            technique.IsAvailable = dto.IsAvailable;
            technique.Location = dto.Location;

            await _repository.UpdateTechniqueAsync(technique);
            return NoContent(); 
        }
    }
}