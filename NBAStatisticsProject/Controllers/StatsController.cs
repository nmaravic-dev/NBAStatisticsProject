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
    public class StatsController : ControllerBase
    {
        private readonly DataContext _context;
        public StatsController(DataContext context)
        {
        _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPlayersSummary()
        {
            var stats = await _context.PlayerGameStats
                .ToStatsSummaryDto()
                .ToListAsync();
            return Ok(stats);
        }
        [HttpGet("summary/{playerId}")]
        public async Task<IActionResult> GetPlayerSummary(int playerId)
        {
            var stats = await _context.PlayerGameStats
                .Where(pgs => pgs.PlayerId == playerId)
                .ToStatsSummaryDto()
                .FirstOrDefaultAsync();
            if (stats == null) return NotFound();
            return Ok(stats);
        }
    }
}
