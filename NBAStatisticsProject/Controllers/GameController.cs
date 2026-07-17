using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NBAStatisticsProject.Data;
using NBAStatisticsProject.DTOs;
using NBAStatisticsProject.Mapping;
using NBAStatisticsProject.Models;

namespace NBAStatisticsProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GameController : ControllerBase
    {
        private readonly DataContext _context;

        public GameController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllGames()
        {
            var games = await _context.Games
                .ToDto()
                .ToListAsync();
            return Ok(games);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetGameById(int id)
        {
            var game = await _context.Games
                .Where(g => g.Id == id)
                .ToDto()
                .FirstOrDefaultAsync();
            if (game == null)
                return NotFound();
            return Ok(game);
        }

        [HttpPost]
        public async Task<IActionResult> AddGame(GameCreateDto gameDto)
        {
            var game = new Game
            {
                Date = gameDto.Date,
                Season = gameDto.Season,
                HomeTeamId = gameDto.HomeTeamId,
                AwayTeamId = gameDto.AwayTeamId,
                HomeScore = gameDto.HomeScore,
                AwayScore = gameDto.AwayScore
            };
            _context.Games.Add(game);
            await _context.SaveChangesAsync();
            var created = await _context.Games
                 .Where(g => g.Id == game.Id)
                 .ToDto()
                 .FirstAsync();

            return CreatedAtAction(nameof(GetGameById), new { id = game.Id }, created);
        }

        [HttpPost("bulk")]
        public async Task<IActionResult> AddGames(List<GameCreateDto> gameDtos)
        {
            var games = gameDtos.Select(gameDto => new Game
            {
                Date = gameDto.Date,
                Season = gameDto.Season,
                HomeTeamId = gameDto.HomeTeamId,
                AwayTeamId = gameDto.AwayTeamId,
                HomeScore = gameDto.HomeScore,
                AwayScore = gameDto.AwayScore
            }).ToList();

            _context.Games.AddRange(games);
            await _context.SaveChangesAsync();
            var ids = games.Select(g => g.Id).ToList();

            var createdGames = await _context.Games
                .Where(g => ids.Contains(g.Id))
                .ToDto()
                .ToListAsync();
            return Ok(createdGames);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateGame(int id, GameCreateDto gameCreateDto)
        {
            var existingGame = await _context.Games.FindAsync(id);
            if (existingGame == null)
                return NotFound();
            existingGame.Date = gameCreateDto.Date;
            existingGame.Season = gameCreateDto.Season;
            existingGame.HomeTeamId = gameCreateDto.HomeTeamId;
            existingGame.AwayTeamId = gameCreateDto.AwayTeamId;
            existingGame.HomeScore = gameCreateDto.HomeScore;
            existingGame.AwayScore = gameCreateDto.AwayScore;
            await _context.SaveChangesAsync();
            var updatedGameDto = await _context.Games
                .Where(g => g.Id == id)
                .ToDto()
                .FirstOrDefaultAsync();

            return Ok(updatedGameDto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGame(int id)
        {
            var game = await _context.Games.FindAsync(id);
            if (game == null)
                return NotFound();
            _context.Games.Remove(game);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
