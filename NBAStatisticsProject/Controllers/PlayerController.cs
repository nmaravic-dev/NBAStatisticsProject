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
    public class PlayerController : ControllerBase
    {
        private readonly DataContext _context;
        public PlayerController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPlayers()
        {
            var players = await _context.Players
                .ToDto()
                .ToListAsync();
            return Ok(players);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPlayersById(int id)
        {
            var player = await _context.Players
                .Where(p => p.Id == id)
                .ToDto()
                .FirstOrDefaultAsync();
            if (player == null)
                return NotFound();
            return Ok(player);
        }

        [HttpGet("team/{teamId}")]
        public async Task<IActionResult> GetPlayersByTeam(int teamId)
        {
            var playersFromSameTeam = await _context.Players
                .Where(p => p.TeamId == teamId)
                .ToDto()
                .ToListAsync();
            return Ok(playersFromSameTeam);
        }

        [HttpGet("players-count")]

        public async Task<IActionResult> NumberOfPlayers()
        {
            var playersCount = await _context.Players.CountAsync();
            return Ok(playersCount);
        }
        [HttpPost]
        public async Task<IActionResult> AddPlayer(PlayerCreateDto playerDto)
        {
            var player = new Player
            {
                Name = playerDto.Name,
                Position = playerDto.Position,
                TeamId = playerDto.TeamId
            };
            _context.Players.Add(player);
            await _context.SaveChangesAsync();
            var createdPlayerDto = await _context.Players
                .Where(p => p.Id == player.Id)
                .ToDto()
                .FirstAsync();
            return CreatedAtAction(nameof(GetPlayersById), new { id = player.Id }, createdPlayerDto);
        }
        [HttpPost("bulk")]
        public async Task<IActionResult> AddPlayers(List<PlayerCreateDto> playerDtos)
        {
            var players = playerDtos.Select(playerDto => new Player
            {
                Name = playerDto.Name,
                Position = playerDto.Position,
                TeamId = playerDto.TeamId
            }).ToList();

            _context.Players.AddRange(players);
            await _context.SaveChangesAsync();
            var ids = players.Select(p => p.Id).ToList();

            var createdPlayers = await _context.Players
                .Where(p => ids.Contains(p.Id))
                .ToDto()
                .ToListAsync();
            return Ok(createdPlayers);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePlayer(int id, PlayerCreateDto playerCreateDto)
        {
            var existingPlayer = await _context.Players.FindAsync(id);
            if (existingPlayer == null)
                return NotFound();

            existingPlayer.Name = playerCreateDto.Name;
            existingPlayer.Position = playerCreateDto.Position;
            existingPlayer.TeamId = playerCreateDto.TeamId;

            await _context.SaveChangesAsync();
            var updatedPlayerDto = await _context.Players
                .Where(p => p.Id == id)
                .ToDto()
                .FirstOrDefaultAsync();
            return Ok(updatedPlayerDto);
        }

        [HttpDelete("{id}")]

        public async Task<IActionResult> DeletePlayer(int id)
        {
            var player = await _context.Players.FindAsync(id);
            if (player == null)
                return NotFound();
            _context.Players.Remove(player);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
