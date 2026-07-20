using Microsoft.AspNetCore.Mvc;
using NBAStatisticsProject.DTOs;
using NBAStatisticsProject.Services;

namespace NBAStatisticsProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlayerGameStatController : ControllerBase
    {
        private readonly IPlayerGameStatService _service;
        public PlayerGameStatController(IPlayerGameStatService service) => _service = service;

        [HttpGet]
        public async Task<IActionResult> GetAllPlayerGameStats()
        {
            var playerGameStats = await _service.GetAllAsync();
            return Ok(playerGameStats);

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPlayerGameStatById(int id)
        {
            var playerGameStat = await _service.GetByIdAsync(id);
            if (playerGameStat == null)
                return NotFound();
            return Ok(playerGameStat);
        }
        [HttpPost]
        public async Task<IActionResult> AddPlayerGameStat(PlayerGameStatCreateDto playerGameStatCreateDto)
        {
            var createdPlayerGameStat = await _service.CreateAsync(playerGameStatCreateDto);
            if (createdPlayerGameStat == null)
                return BadRequest("Invalid player or game ID.");
            return CreatedAtAction(nameof(GetPlayerGameStatById), new { id = createdPlayerGameStat.Id }, createdPlayerGameStat);
        }
        [HttpPost("bulk")]
        public async Task<IActionResult> AddPlayerGameStats(List<PlayerGameStatCreateDto> playerGameStatDtos)
        {
            var createdPlayerGameStats = await _service.CreateManyAsync(playerGameStatDtos);
            return Ok(createdPlayerGameStats);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePlayerGameStat(int id, PlayerGameStatCreateDto playerGameStatCreateDto)
        {
            var updatedPlayerGameStat = await _service.UpdateAsync(id, playerGameStatCreateDto);
            if (updatedPlayerGameStat == null)
                return BadRequest("Invalid player or game ID.");
            return Ok(updatedPlayerGameStat);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePlayerGameStat(int id)
        {
            var result = await _service.DeleteAsync(id);
            if (!result)
                return NotFound();
            return NoContent();
        }
    }
}
