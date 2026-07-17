using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NBAStatisticsProject.Data;
using NBAStatisticsProject.DTOs;
using NBAStatisticsProject.Models;
using NBAStatisticsProject.Mapping;

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
                .ToDto()
                .ToListAsync();
            return Ok(playerGameStats);

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPlayerGameStatById(int id)
        {
            var playerGameStat = await _context.PlayerGameStats
                .Where(pgs => pgs.Id == id)
                .ToDto()
                .FirstOrDefaultAsync();            
            if (playerGameStat == null)
                return NotFound();
            return Ok(playerGameStat);
        }

        [HttpPost]
        public async Task<IActionResult> AddPlayerGameStat(PlayerGameStatCreateDto playerGameStatCreateDto)
        {
            var playerGameStat = new PlayerGameStat
            {
                PlayerId = playerGameStatCreateDto.PlayerId,
                GameId = playerGameStatCreateDto.GameId,
                Points = playerGameStatCreateDto.Points,
                Assists = playerGameStatCreateDto.Assists,
                Rebounds = playerGameStatCreateDto.Rebounds,
                MinutesPlayed = playerGameStatCreateDto.MinutesPlayed,
                Steals = playerGameStatCreateDto.Steals,
                Blocks = playerGameStatCreateDto.Blocks,
                Turnovers = playerGameStatCreateDto.Turnovers
            };
            _context.PlayerGameStats.Add(playerGameStat);
            await _context.SaveChangesAsync();
            var createdPlayerGameStat = await _context.PlayerGameStats
                .Where(pgs => pgs.Id == playerGameStat.Id)
                .ToDto()
                .FirstAsync();
            return CreatedAtAction(nameof(GetPlayerGameStatById), new { id = playerGameStat.Id }, createdPlayerGameStat);
        }
        [HttpPost("bulk")]
        public async Task<IActionResult> AddPlayerGameStats(List<PlayerGameStatCreateDto> playerGameStatDtos)
        {
            var playerGameStats = playerGameStatDtos.Select(pgsDto => new PlayerGameStat
            {
                PlayerId = pgsDto.PlayerId,
                GameId = pgsDto.GameId,
                Points = pgsDto.Points,
                Assists = pgsDto.Assists,
                Rebounds = pgsDto.Rebounds,
                MinutesPlayed = pgsDto.MinutesPlayed,
                Steals = pgsDto.Steals,
                Blocks = pgsDto.Blocks,
                Turnovers = pgsDto.Turnovers
            }).ToList();

            _context.PlayerGameStats.AddRange(playerGameStats);
            await _context.SaveChangesAsync();
            var ids = playerGameStats.Select(pgs => pgs.Id).ToList();

            var createdPlayerGameStats = await _context.PlayerGameStats
                .Where(pgs => ids.Contains(pgs.Id))
                .ToDto()
                .ToListAsync();
            return Ok(createdPlayerGameStats);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePlayerGameStat(int id, PlayerGameStatCreateDto playerGameStatCreateDto)
        {
            var existingPlayerGameStat = await _context.PlayerGameStats.FindAsync(id);
            if (existingPlayerGameStat == null)
                return NotFound();
            existingPlayerGameStat.PlayerId = playerGameStatCreateDto.PlayerId;
            existingPlayerGameStat.GameId = playerGameStatCreateDto.GameId;
            existingPlayerGameStat.Points = playerGameStatCreateDto.Points;
            existingPlayerGameStat.Assists = playerGameStatCreateDto.Assists;
            existingPlayerGameStat.Rebounds = playerGameStatCreateDto.Rebounds;
            existingPlayerGameStat.MinutesPlayed = playerGameStatCreateDto.MinutesPlayed;
            existingPlayerGameStat.Steals = playerGameStatCreateDto.Steals;
            existingPlayerGameStat.Blocks = playerGameStatCreateDto.Blocks;
            existingPlayerGameStat.Turnovers = playerGameStatCreateDto.Turnovers;
            await _context.SaveChangesAsync();
            var updatedPlayerGameStat = await _context.PlayerGameStats
                .Where(pgs => pgs.Id == id)
                .ToDto()
                .FirstAsync();
            return Ok(updatedPlayerGameStat);
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
