using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NBAStatisticsProject.Data;
using NBAStatisticsProject.Models;

namespace NBAStatisticsProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlayerGameStatController : ControllerBase
    {
        private readonly DataContext _context;
        public PlayerGameStatController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPlayerGameStats()
        {
            var playerGameStats = await _context.PlayerGameStats
                .Include(pgs => pgs.Player)
                .Include(pgs => pgs.Game)
                .ToListAsync();
            return Ok(playerGameStats);

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPlayerGameStatById(int id)
        {
            var playerGameStat = await _context.PlayerGameStats
                .Include(pgs => pgs.Player)
                .Include(pgs => pgs.Game)
                .FirstOrDefaultAsync(pgs => pgs.Id == id);
            if (playerGameStat == null)
                return NotFound();
            return Ok(playerGameStat);
        }

        [HttpPost]
        public async Task<IActionResult> AddPlayerGameStat(PlayerGameStat playerGameStat)
        {
            _context.PlayerGameStats.Add(playerGameStat);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetPlayerGameStatById), new { id = playerGameStat.Id }, playerGameStat);
        }
        [HttpPost("bulk")]
        public async Task<IActionResult> AddPlayerGameStats(List<PlayerGameStat> playerGameStats)
        {
            _context.PlayerGameStats.AddRange(playerGameStats);
            await _context.SaveChangesAsync();
            return Ok(playerGameStats);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePlayerGameStat(int id, PlayerGameStat playerGameStat)
        {
            var existingPlayerGameStat = await _context.PlayerGameStats.FindAsync(id);
            if (existingPlayerGameStat == null)
                return NotFound();
            existingPlayerGameStat.PlayerId = playerGameStat.PlayerId;
            existingPlayerGameStat.GameId = playerGameStat.GameId;
            existingPlayerGameStat.Points = playerGameStat.Points;
            existingPlayerGameStat.Assists = playerGameStat.Assists;
            existingPlayerGameStat.Rebounds = playerGameStat.Rebounds;
            existingPlayerGameStat.MinutesPlayed = playerGameStat.MinutesPlayed;
            existingPlayerGameStat.Steals = playerGameStat.Steals;
            existingPlayerGameStat.Blocks = playerGameStat.Blocks;
            existingPlayerGameStat.Turnovers = playerGameStat.Turnovers;
            await _context.SaveChangesAsync();
            return Ok(existingPlayerGameStat);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePlayerGameStat(int id)
        {
            var existingPlayerGameStat = await _context.PlayerGameStats.FindAsync(id);
            if (existingPlayerGameStat == null)
                return NotFound();
            _context.PlayerGameStats.Remove(existingPlayerGameStat);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
