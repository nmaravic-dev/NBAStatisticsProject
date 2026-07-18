using NBAStatisticsProject.Models;
using NBAStatisticsProject.DTOs;

namespace NBAStatisticsProject.Mapping
{
    public static class StatsMappingExtensions
    {
        public static IQueryable<StatsSummaryDto> ToStatsSummaryDto(this IQueryable<PlayerGameStat> query)
        {
            return query
                .GroupBy(s => new { s.PlayerId, s.Player!.Name })
                .Select(g => new StatsSummaryDto(
                    g.Key.PlayerId,
                    g.Count(),
                    g.Key.Name,
                    g.Sum(s => s.Points),
                    g.Sum(s => s.Assists),
                    g.Sum(s => s.Rebounds),
                    g.Average(s => (double)s.Points),
                    g.Average(s => (double)s.Assists),
                    g.Average(s => (double)s.Rebounds)
                ));
        }
    }
}
