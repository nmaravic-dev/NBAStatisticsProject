using Microsoft.EntityFrameworkCore;
using NBAStatisticsProject.Data;
using NBAStatisticsProject.DTOs;

namespace NBAStatisticsProject.Services
{
    public class InjuryScoreService : IInjuryScoreService
    {
        private readonly DataContext _context;
        private readonly ILogger<InjuryScoreService> _logger;
        public InjuryScoreService(DataContext context, ILogger<InjuryScoreService> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<InjuryScoreDto?> GetInjuryScoreAsync(int playerId)
        {
            var player = await _context.Players.FindAsync(playerId);
            if (player == null)
            {
                _logger.LogWarning("Injury score requested for non-existent player {PlayerId}", playerId);
                return null; 
            }
            var injuries = await _context.Injuries
                .Where(i => i.PlayerId == playerId)
                .ToListAsync();
            if (injuries.Count == 0)
                return new InjuryScoreDto(playerId, player.Name, 0, 0, 0, 10.0);
            int totalMissedGames = 0;
            int weightedMissedGames = 0;

            var gameDates = await _context.Games
                .Where(g => g.HomeTeamId == player.TeamId || g.AwayTeamId == player.TeamId)
                .Select(g => g.Date)
                .ToListAsync();
            foreach (var i in injuries)
            {
                var end = i.EndDate ?? DateTime.Now;
                var missed = gameDates.Count(d => d >= i.StartDate && d <= end);
                totalMissedGames += missed;
                weightedMissedGames += missed * (int)i.Severity;
            }

            int totalDaysInjured = injuries.Sum(i =>(int)((i.EndDate ?? DateTime.Now) - i.StartDate).TotalDays);

            var playedGames = await _context.PlayerGameStats
                .CountAsync(pgs => pgs.PlayerId == playerId);

            double score = CalculateInjuryScore(playedGames, weightedMissedGames);

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
            var scores = new List<InjuryScoreDto>();

            var players = await _context.Players
                .Select(p => new { p.Id, p.Name, p.TeamId })
                .ToListAsync();

            var injuries = await _context.Injuries.ToListAsync();

            var games = await _context.Games
                .Select(g => new { g.Date, g.HomeTeamId, g.AwayTeamId })
                .ToListAsync();

            var playedByPlayer = (await _context.PlayerGameStats
                    .GroupBy(s => s.PlayerId)
                    .Select(g => new { PlayerId = g.Key, Games = g.Count() })
                    .ToListAsync())
                .ToDictionary(x => x.PlayerId, x => x.Games);

            var injuriesByPlayer = injuries.ToLookup(i => i.PlayerId);

            var gameDatesByTeam = games
                .SelectMany(g => new[]
                {
            new { TeamId = g.HomeTeamId, g.Date },
            new { TeamId = g.AwayTeamId, g.Date }
                })
                .ToLookup(x => x.TeamId, x => x.Date);

            foreach (var p in players)
            {
                var personalInjuries = injuriesByPlayer[p.Id];
                if (!personalInjuries.Any())
                {
                    scores.Add(new InjuryScoreDto(p.Id, p.Name, 0, 0, 0, 10.0));
                    continue;
                }

                var teamGameDates = gameDatesByTeam[p.TeamId];

                int totalMissedGames = 0;
                int weightedMissedGames = 0;

                foreach (var i in personalInjuries)
                {
                    var end = i.EndDate ?? DateTime.Now;
                    var missed = teamGameDates.Count(d => d >= i.StartDate && d <= end);
                    totalMissedGames += missed;
                    weightedMissedGames += missed * (int)i.Severity;
                }

                int totalDaysInjured = personalInjuries
                    .Sum(i => (int)((i.EndDate ?? DateTime.Now) - i.StartDate).TotalDays);

                int playedGames = playedByPlayer.GetValueOrDefault(p.Id, 0);

                double score = CalculateInjuryScore(playedGames, weightedMissedGames);

                scores.Add(new InjuryScoreDto(
                    p.Id, p.Name, personalInjuries.Count(),
                    totalDaysInjured, totalMissedGames, score));
            }

            return scores;
        }
        private static double CalculateInjuryScore(int playedGames, int weightedMissedGames)
        {
            if (playedGames + weightedMissedGames == 0) return 10.0;
            double availability = (double)playedGames / (playedGames + weightedMissedGames);
            return Math.Round(availability * 10, 1);
        }

    }
}
