using Microsoft.AspNetCore.Mvc;
using NBAStatisticsProject.DTOs;
using NBAStatisticsProject.Services;

namespace NBAStatisticsProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlayerController : ControllerBase
    {
        private readonly IPlayerService _service;
        public PlayerController(IPlayerService service) => _service = service;

        [HttpGet]
        public async Task<IActionResult> GetAllPlayers()
        {
            var players = await _service.GetAllAsync();
            return Ok(players);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPlayersById(int id)
        {
            var player = await _service.GetByIdAsync(id);
            if (player == null)
                return NotFound();
            return Ok(player);
        }

        [HttpGet("team/{teamId}")]
        public async Task<IActionResult> GetPlayersByTeam(int teamId)
        {
            var playersFromSameTeam = await _service.GetPlayersByTeamAsync(teamId);
            return Ok(playersFromSameTeam);
        }

        [HttpGet("players-count")]

        public async Task<IActionResult> NumberOfPlayers()
        {
            var playersCount = await _service.GetCountAsync();
            return Ok(playersCount);
        }
        [HttpPost]
        public async Task<IActionResult> AddPlayer(PlayerCreateDto playerDto)
        {
            var createdPlayer = await _service.CreateAsync(playerDto);
            return CreatedAtAction(nameof(GetPlayersById), new { id = createdPlayer.Id }, createdPlayer);
        }
        [HttpPost("bulk")]
        public async Task<IActionResult> AddPlayers(List<PlayerCreateDto> playerDtos)
        {
            var createdPlayers = await _service.CreateManyAsync(playerDtos);
            return Ok(createdPlayers);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePlayer(int id, PlayerCreateDto playerCreateDto)
        {
            var updatedPlayer = await _service.UpdateAsync(id, playerCreateDto);
            if (updatedPlayer == null)
                return NotFound();
            return Ok(updatedPlayer);
        }

        [HttpDelete("{id}")]

        public async Task<IActionResult> DeletePlayer(int id)
        {
            var result = await _service.DeleteAsync(id);
            if (!result)
                return NotFound();
            return NoContent();
        }
    }
}
