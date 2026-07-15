using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NBAStatisticsProject.Data;
using NBAStatisticsProject.Models;

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
            var players = await _context.Players.ToListAsync();
            return Ok(players);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPlayersById(int id)
        {
            var player = await _context.Players.FindAsync(id);
            if (player == null)
                return NotFound();
            return Ok(player);
        }

        [HttpGet("team/{teamId}")]
        public async Task<IActionResult> GetPlayersByTeam(int teamId)
        {
            var playersFromSameTeam = await _context.Players.Where(p => p.TeamId == teamId).ToListAsync();
            return Ok(playersFromSameTeam);
        }

        [HttpGet("players-count")]

        public async Task<IActionResult> NumberOfPlayers()
        {
            var playersCount = await _context.Players.CountAsync();
            return Ok(playersCount);
        }
        [HttpPost]
        public async Task<IActionResult> AddPlayer(Player player)
        {
            _context.Players.Add(player);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetPlayersById), new { id = player.Id }, player);
        }
        [HttpPost("bulk")]
        public async Task<IActionResult> AddPlayers(List<Player> players)
        {
            _context.Players.AddRange(players);   
            await _context.SaveChangesAsync();
            return Ok(players);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> ChangePlayersStats(int id, [FromBody] Player updatedPlayer)
        {
            var existingPlayer = await _context.Players.FindAsync(id);
            if (existingPlayer == null)
                return NotFound();

            existingPlayer.Name = updatedPlayer.Name;
            existingPlayer.Position = updatedPlayer.Position;
            existingPlayer.TeamId = updatedPlayer.TeamId;

            await _context.SaveChangesAsync();
            return Ok(existingPlayer);

        }

        [HttpPatch("{id}")]

        public async Task<IActionResult> ChangeIndividualStat(int id, [FromBody] JsonPatchDocument<Player> patchDoc)
        {
            var player = await _context.Players.FindAsync(id);
            if (player == null) return NotFound();

            patchDoc.ApplyTo(player, ModelState);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            await _context.SaveChangesAsync();
            return Ok(player);
        }

        [HttpDelete("{id}")]

        public async Task<IActionResult> DeletePlayer(int id)
        {
            var player = await _context.Players.FindAsync(id);
            if (player == null)
                return NotFound();
            _context.Players.Remove(player);
            await _context.SaveChangesAsync();
            return Ok(player);
        }
    }
}
