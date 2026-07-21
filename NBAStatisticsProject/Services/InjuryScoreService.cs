using Microsoft.EntityFrameworkCore;
using NBAStatisticsProject.Data;
using NBAStatisticsProject.DTOs;
using NBAStatisticsProject.Mapping;
using NBAStatisticsProject.Models;

namespace NBAStatisticsProject.Services
{
    public class InjuryScoreService : IInjuryScoreService
    {
        private readonly DataContext _context;
        public InjuryScoreService(DataContext context) => _context = context;
        public async Task<InjuryScoreDto?> GetInjuryScoreAsync(int playerId)
        {
            var player = await _context.Players.FindAsync(playerId);
            if (player == null) return null;
            var injuries = await _context.Injuries
                .Where(i => i.PlayerId == playerId)
                .ToListAsync();
            if (injuries.Count == 0)
                return new InjuryScoreDto(playerId, player.Name, 0, 0, 0, 10.0);
            int totalMissedGames = 0;
            int weightedMissedGames = 0;
            foreach (var i in injuries)
            {
                var missed = await _context.Games
                    .CountAsync(g =>
                    (g.HomeTeamId == player.TeamId || g.AwayTeamId == player.TeamId)  
                    && g.Date >= i.StartDate                                          
                    && g.Date <= (i.EndDate ?? DateTime.UtcNow));
                totalMissedGames += missed;
                weightedMissedGames += missed * (int)i.Severity;
            }

            int totalDaysInjured = injuries.Sum(i =>(int)((i.EndDate ?? DateTime.UtcNow) - i.StartDate).TotalDays);

            var playedGames = await _context.PlayerGameStats
                .CountAsync(pgs => pgs.PlayerId == playerId);

            double score;

            if (playedGames + weightedMissedGames == 0)
                score = 10.0; 
            else
            {
                double availability = (double)playedGames / (playedGames + weightedMissedGames);
                score = Math.Round(availability * 10, 1);
            }

            return new InjuryScoreDto(
                player.Id,
                player.Name,
                injuries.Count,
                totalDaysInjured,
                totalMissedGames,
                score

            );
            
        }
        public async Task<List<InjuryScoreDto>> GetAllInjuryScoresAsync()
        {
            var playerIds = await _context.Players.Select(p => p.Id).ToListAsync();
            var scores = new List<InjuryScoreDto>();
            foreach (var id in playerIds)
            {
                var score = await GetInjuryScoreAsync(id);
                if (score != null) scores.Add(score);
            }
            return scores;
        }

    }
}
