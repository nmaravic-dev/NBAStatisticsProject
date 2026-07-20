using Microsoft.AspNetCore.Mvc;
using NBAStatisticsProject.Services;

namespace NBAStatisticsProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AnalyticsController : ControllerBase
    {
        private readonly IAnalyticsService _service;
        public AnalyticsController(IAnalyticsService service) => _service = service;

        [HttpGet]
        public async Task<IActionResult> GetAllPlayersSummary()
        {
            var stats = await _service.GetAllStatsSummaryAsync();
            return Ok(stats);
        }
        [HttpGet("summary/{playerId}")]
        public async Task<IActionResult> GetPlayerSummary(int playerId)
        {
            var stats = await _service.GetPlayerSummaryAsync(playerId);
            if (stats == null) return NotFound();
            return Ok(stats);
        }
    }
}
