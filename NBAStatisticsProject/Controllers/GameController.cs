using Microsoft.AspNetCore.Mvc;
using NBAStatisticsProject.DTOs;
using NBAStatisticsProject.Services;

namespace NBAStatisticsProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GameController : ControllerBase
    {
        private readonly IGameService _service;
        public GameController(IGameService service) => _service = service;

        [HttpGet]
        public async Task<IActionResult> GetAllGames()
        {
            var games = await _service.GetAllAsync();
            return Ok(games);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetGameById(int id)
        {
            var game = await _service.GetByIdAsync(id);
            if (game == null)
                return NotFound();
            return Ok(game);
        }

        [HttpPost]
        public async Task<IActionResult> AddGame(GameCreateDto gameDto)
        {
            var createdGame = await _service.CreateAsync(gameDto);
            if (createdGame == null)
                return BadRequest("Invalid game data: check team IDs, date, or duplicate teams.");
            return CreatedAtAction(nameof(GetGameById), new { id = createdGame.Id }, createdGame);
        }
        [HttpPost("bulk")]
        public async Task<IActionResult> AddGames(List<GameCreateDto> gameDtos)
        {
            var createdGames = await _service.CreateManyAsync(gameDtos);
            return Ok(createdGames);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateGame(int id, GameCreateDto gameCreateDto)
        {
            var updatedGame = await _service.UpdateAsync(id, gameCreateDto);
            if (updatedGame == null)
                return NotFound();
            return Ok(updatedGame);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGame(int id)
        {
            var result = await _service.DeleteAsync(id);
            if (!result)
                return NotFound();
            return NoContent();
        }
    }
}
