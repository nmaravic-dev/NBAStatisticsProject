using Microsoft.AspNetCore.Mvc;
using NBAStatisticsProject.Services;

namespace NBAStatisticsProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AnalyticsController : ControllerBase
    {
        private readonly IAnalyticsService _service;
        private readonly IInjuryScoreService _injuryScoreService;
        public AnalyticsController(IAnalyticsService service, IInjuryScoreService injuryScoreService)

        {
            _service = service;
            _injuryScoreService = injuryScoreService;
        }
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
        [HttpGet("injury-score")]
        public async Task<IActionResult> GetAllInjuryScores()
        {
            var scores = await _injuryScoreService.GetAllInjuryScoresAsync();
            return Ok(scores);
        }

        [HttpGet("injury-score/{playerId}")]
        public async Task<IActionResult> GetInjuryScore(int playerId)
        {
            var score = await _injuryScoreService.GetInjuryScoreAsync(playerId);
            if (score == null) return NotFound();
            return Ok(score);
        }
    }
}
