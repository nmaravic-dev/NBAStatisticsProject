using Microsoft.AspNetCore.Mvc;
using NBAStatisticsProject.DTOs;
using NBAStatisticsProject.Services;

namespace NBAStatisticsProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InjuryController : ControllerBase
    {
        private readonly IInjuryService _service;
        public InjuryController(IInjuryService service) => _service = service;
        [HttpGet]
        public async Task<IActionResult> GetAllInjuries()
        {
            var injuries = await _service.GetAllAsync();
            return Ok(injuries);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetInjuryById(int id) 
        {
            var injury = await _service.GetByIdAsync(id);
            if (injury == null)
                return NotFound();
            return Ok(injury);
        }

        [HttpPost]
        public async Task<IActionResult> AddInjury(InjuryCreateDto injuryDto)
        {
            var createdInjury = await _service.CreateAsync(injuryDto);
            if (createdInjury == null)
                return BadRequest("Invalid injury data");
            return CreatedAtAction(nameof(GetInjuryById), new { id = createdInjury.Id }, createdInjury);
        }

        [HttpPost("bulk")]
        public async Task<IActionResult> AddInjuries(List<InjuryCreateDto> injuryDtos)
        {
            var createdInjuries = await _service.CreateManyAsync(injuryDtos);
            return Ok(createdInjuries);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateInjury(int id, InjuryCreateDto injuryDto)
        {
            var updatedInjury = await _service.UpdateAsync(id, injuryDto);
            if (updatedInjury == null)
                return BadRequest("Invalid injury data");
            return Ok(updatedInjury);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInjury(int id) 
        {
            var result = await _service.DeleteAsync(id);
            if (!result)
                return NotFound();
            return NoContent();
        }
    }
}
