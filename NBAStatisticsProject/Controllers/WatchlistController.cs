using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NBAStatisticsProject.DTOs;
using NBAStatisticsProject.Services;
using System.Security.Claims;

namespace NBAStatisticsProject.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class WatchlistController : ControllerBase
    {
        private readonly IWatchlistService _service;
        public WatchlistController(IWatchlistService service) => _service = service;

        [HttpGet]
        public async Task<IActionResult> GetWatchlist ()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();
            var watchlist = await _service.GetAllAsync(userId);
            return Ok(watchlist);
        }
        [HttpPost]

        public async Task<IActionResult> AddPlayer( WatchlistEntryCreateDto dto) 
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();
            var addPlayer = await _service.CreateAsync(userId, dto);
            if (addPlayer == null) 
                return BadRequest("The player already exist in your watchlist!");
            return Ok(addPlayer);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePlayer(int id) 
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();
            var result = await _service.DeleteAsync(userId, id);
            if (!result) 
                return NotFound();
            return NoContent();
        }
    }
}
