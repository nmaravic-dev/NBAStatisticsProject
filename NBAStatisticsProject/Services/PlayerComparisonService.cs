using NBAStatisticsProject.DTOs;
using NBAStatisticsProject.Models;
using Microsoft.EntityFrameworkCore;
using NBAStatisticsProject.Data;

namespace NBAStatisticsProject.Services
{
    public class PlayerComparisonService :IPlayerComparisonService
    {
        private readonly DataContext _context;
        public PlayerComparisonService(DataContext context) => _context = context;
        public async Task<ComparisonDto?> ComparePlayersAsync(int playerAId, int playerBId)
        {
            var players = await _context.Players
                .Where(p => p.Id == playerAId || p.Id == playerBId)
                .ToListAsync();
            if (players.Count != 2) return null;

            var playerA = players.First(p => p.Id == playerAId);
            var playerB = players.First(p => p.Id == playerBId);

            var stats = await _context.PlayerGameStats
                .Where(s => s.PlayerId == playerAId || s.PlayerId == playerBId)
                .ToListAsync();

            var statsA = stats.Where(s => s.PlayerId == playerAId).ToList();
            var statsB = stats.Where(s => s.PlayerId == playerBId).ToList();

            return new ComparisonDto(
            BuildComparison(playerA, statsA),
            BuildComparison(playerB, statsB)        
            );
        }
        private static PlayerComparisonDto BuildComparison(Player player, List<PlayerGameStat> stats)
        {
            if (stats.Count == 0)
                return new PlayerComparisonDto(player.Id, $"{player.FirstName} {player.LastName}", 0, 0, 0, 0, 0, 0, 0, 0);

            return new PlayerComparisonDto(
                PlayerId: player.Id,
                PlayerName: $"{player.FirstName} {player.LastName}",
                GamesPlayed: stats.Count,
                PointsPerGame: stats.Average(s => s.Points),
                AssistsPerGame: stats.Average(s => s.Assists),
                ReboundsPerGame: stats.Average(s => s.Rebounds),
                MinutesPlayedPerGame: stats.Average(s => s.MinutesPlayed),
                StealsPerGame: stats.Average(s => s.Steals),
                BlocksPerGame: stats.Average(s => s.Blocks),
                TurnoversPerGame: stats.Average(s => s.Turnovers)
            );
        }
    }
}
