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
        private readonly IPlayerComparisonService _playerComparisonService;
        public AnalyticsController(IAnalyticsService service, IInjuryScoreService injuryScoreService, IPlayerComparisonService playerComparisonService)
        {
            _service = service;
            _injuryScoreService = injuryScoreService;
            _playerComparisonService = playerComparisonService;
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

        [HttpGet("compare")]
        public async Task<IActionResult> ComparePlayers(int playerAId, int playerBId)
        {
            if (playerAId == playerBId) return BadRequest("Players must be different.");
            var comparison = await _playerComparisonService.ComparePlayersAsync(playerAId, playerBId);
            if (comparison == null) return NotFound();
            return Ok(comparison);
        }
    }
}
