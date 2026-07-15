using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NBAStatisticsProject.Data;
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
                .Include(g => g.HomeTeam)
                .Include(g => g.AwayTeam)
                .ToListAsync();
            return Ok(games);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetGameById(int id)
        {
            var game = await _context.Games
                .Include(g => g.HomeTeam)
                .Include(g => g.AwayTeam)
                .FirstOrDefaultAsync(g => g.Id == id);
            if (game == null)
                return NotFound();
            return Ok(game);
        }

        [HttpPost]
        public async Task<IActionResult> AddGame(Game game)
        {
            _context.Games.Add(game);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetGameById), new { id = game.Id }, game);
        }

        [HttpPost("bulk")]
        public async Task<IActionResult> AddGames(List<Game> games)
        {
            _context.Games.AddRange(games);
            await _context.SaveChangesAsync();
            return Ok(games);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateGame(int id, Game game)
        {
            var existingGame = await _context.Games.FindAsync(id);
            if (existingGame == null)
                return NotFound();
            existingGame.Date = game.Date;
            existingGame.Season = game.Season;
            existingGame.HomeTeamId = game.HomeTeamId;
            existingGame.AwayTeamId = game.AwayTeamId;
            existingGame.HomeScore = game.HomeScore;
            existingGame.AwayScore = game.AwayScore;
            await _context.SaveChangesAsync();
            return Ok(existingGame);
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
